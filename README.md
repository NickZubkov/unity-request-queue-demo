# Unity Request Queue Demo

Небольшое Unity-приложение, демонстрирующее работу с бэкендом через **очередь последовательных запросов** с поддержкой отмены и чистую архитектуру на Zenject + UniTask + UniRx.

Три вкладки с нижней навигацией: **Кликер**, **Погода**, **Породы собак**. Ядро проекта — очередь сетевых запросов; на ней сфокусированы и код, и тесты.

## Стек

- Unity **2022.3.62f3**
- Zenject (DI, без синглтонов)
- UniTask + `CancellationToken` (асинхронность и отмена)
- UniRx (реактивное состояние моделей)
- DOTween (анимации VFX/попапа)
- UnityWebRequest (сеть)
- Newtonsoft.Json (десериализация JSON:API)
- ScriptableObject (все игровые параметры и конфигурация сети)
- Unity Test Framework (EditMode-тесты)

## Запуск

1. Открыть проект в Unity **2022.3.62f3** (версия жёстко привязана: `ProjectSettings/ProjectVersion.txt`).
2. Установить **DOTween**: Asset Store → импорт `.unitypackage` → `Tools → Demigiant → DOTween Utility Panel → Setup DOTween…` → в той же панели **Create ASMDEF** (без сборки `DOTween.Modules` наши asmdef не увидят DOTween).
3. Убедиться, что подтянулся `com.unity.nuget.newtonsoft-json` (Package Manager → при необходимости Resolve).
4. Открыть сцену `Assets/Scenes/Main.unity` и нажать **Play**.

**Тесты:** `Window → General → Test Runner → EditMode → Run All` (9 тестов: 6 на очередь + 3 на десериализацию DTO). Headless-вариант:

```
Unity.exe -runTests -batchmode -projectPath "<путь к проекту>" -testPlatform EditMode -testResults results.xml -logFile -
```

## Структура проекта

Три сборки (asmdef); ключевой принцип — **ядро не зависит ни от Zenject, ни от UI** и проверяется юнит-тестами изолированно.

```
Assets/Scripts/
  Core/   (RequestQueueDemo.Core)   — очередь, сетевой слой, DTO, domain-модели, мапперы.
                                       Ссылки: UniTask, UniRx, Newtonsoft.Json. Без Zenject/UI.
  App/    (RequestQueueDemo.App)    — Zenject-инсталлеры, навигация, 3 вкладки (MVP),
                                       общий UI (попап, лоадер), VFX, пулы.
  Tests/EditMode/ (RequestQueueDemo.Tests.EditMode) — тесты очереди и десериализации.
Assets/Scenes/    Main.unity
Assets/Prefabs/   FloatingText, TapParticle, BreedListItem, Popup
Assets/Settings/  ApiConfig.asset, ClickerConfig.asset  (ScriptableObject-конфиги)
```

Карта неймспейсов: `Core.Queue`, `Core.Network`, `Core.Network.Dto`, `Core.Domain`, `App.Installers`, `App.Navigation`, `App.Common.Ui`, `App.Features.{Clicker,Weather,Breeds}`.

## Архитектура

- **MVP + UniRx.** View пассивный, за узким интерфейсом (`IClickerView`, `IWeatherView`, `IBreedsView`); логика — в Presenter; состояние — в Model на `ReactiveProperty`. Общей MVP-иерархии (маркер-интерфейсов/базового презентера) сознательно нет — ради трёх вкладок это преждевременно.
- **DI — Zenject, без синглтонов.** `ProjectContext → ProjectInstaller` (очередь, транспорт, конфиг сети, кэш — project-scope); `SceneContext → GameInstaller` (навигация, презентеры/вью вкладок, `ClickerConfig`, пулы). `AsSingle()` здесь — DI-scope, а не Singleton-паттерн.
- **Пулинг** часто создаваемых объектов — Zenject `MemoryPool` (`FloatingText`, `TapParticle`, `BreedListItem`).
- **Конфигурация — только ScriptableObject** (`ApiConfig`, `ClickerConfig`): URL, заголовки, интервалы, награды, стоимости, кап энергии — без хардкода в логике.
- **Навигация:** `NavigationController` (`IInitializable`) переключает вкладки; презентер каждой вкладки реализует `ITab` (`OnEnter`/`OnExit`) — это точки запуска/остановки задач и отмены запросов.
- **UI адаптивный** (мобильный портрет): `CanvasScaler` Scale With Screen Size, reference 1080×1920, Match 0.5; раскладка на якорях/`LayoutGroup`/`ContentSizeFitter`.

## Ядро: очередь запросов

`IRequestQueue.EnqueueAsync<T>(IRequestOperation<T>, CancellationToken)` — ставит операцию в очередь и возвращает её результат. Реализация `RequestQueue`:

- **Последовательное исполнение.** В любой момент выполняется ровно одна операция; следующая стартует только после завершения предыдущей. Внутри — `LinkedList<IQueueItem>` и один процессор-петля (`UniTaskVoid`, fire-and-forget).
- **Единый механизм отмены — через `CancellationToken`** вызывающего. Два сценария ТЗ покрываются одним токеном:
  - *не стартовал (Pending)* → элемент удаляется из очереди (коллбек `token.Register`, синхронно на главном потоке);
  - *выполняется (Running)* → linked-токен внутри `RunAsync` отменяется, `UnityWebRequest.WithCancellation` прерывает запрос сам, результат — `Canceled`, процессор идёт дальше.
- **Изоляция ошибок.** Исключение операции оседает в её `UniTaskCompletionSource` и не валит процессор — следующие запросы исполняются.
- **Single-thread affinity, без `lock`.** Очередь рассчитана на главный поток Unity: UniTask-продолжения возвращаются на `PlayerLoop`, отмена вызывается из презентеров (тоже главный поток). Истинной параллельности нет → синхронизация не нужна; корректность держится на кооперативной модели между точками `await`.
- **Завершение жизни.** `RequestQueue : IDisposable`: `Dispose` отменяет выполняющийся (через shutdown-токен) и снимает все Pending, очищает очередь.

Отмена на уровне вкладки: у презентера свой `CancellationTokenSource` (создаётся в `OnEnter`, отменяется в `OnExit`), все запросы вкладки идут с его токеном — уход с вкладки отменяет выполняющийся и удаляет ожидающие.

Ядро покрыто **6 EditMode-тестами** (последовательность, проброс результата, изоляция ошибки, отмена Running, удаление Pending, отмена при Dispose) на детерминированной фейк-операции с ручным завершением — без реального времени и сети. Ещё **3 теста** проверяют десериализацию DTO → domain (weather `periods[0]`, список пород с лимитом, факты породы).

## Вкладки

1. **Кликер** (без сети) — валюта по тапу и автосбором, система энергии (трата/реген с капом), VFX (партикл-пул, летящий «+1» на DOTween, punch кнопки, слот звука). Все параметры — из `ClickerConfig`. Автосбор и реген работают всегда (idle-накопление на любой вкладке).
2. **Погода** — петля запросов каждые 5 с через очередь; уход с вкладки отменяет/удаляет запрос. Иконка грузится по URL (`UnityWebRequestTexture`) с кэшем по URL и фолбэком при ошибке. Вывод: «{name} - {temp}{unit}» (напр. «Today - 61F»).
3. **Породы собак** — список 10 пород через пул с лоадером; клик по породе → факты с лоадером и попапом (адаптивная высота через `VerticalLayoutGroup` + `ContentSizeFitter`). Клик по другой породе отменяет предыдущий запрос фактов; уход с вкладки отменяет любой запрос (linked-токены).

Сетевые ошибки и отмена обрабатываются — приложение не падает (`OperationCanceledException` гасится, прочие ошибки логируются и пропускают тик).

## Что улучшил бы при наличии времени

- Персистентность баланса/энергии (PlayerPrefs/сериализация) — сейчас in-memory.
- Ретраи с backoff и таймауты на уровне `IWebRequestRunner`.
- Приоритеты и дедупликация запросов в очереди.
- Локализация (вынести строки, перевести метки погоды — сейчас `name` из API, напр. «Today»).
- Play Mode тесты на сценарии вкладок (отмена при переключении) и юнит-тест операции с фейк-`IWebRequestRunner`.
- Addressables вместо прямых ссылок на префабы.

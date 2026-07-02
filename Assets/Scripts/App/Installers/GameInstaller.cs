using RequestQueueDemo.App.Config;
using RequestQueueDemo.App.Features.Breeds;
using RequestQueueDemo.App.Features.Clicker;
using RequestQueueDemo.App.Features.Weather;
using RequestQueueDemo.App.Navigation;
using UnityEngine;
using Zenject;

namespace RequestQueueDemo.App.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private ClickerConfig _clickerConfig;
        [SerializeField] private ClickerView _clickerView;
        [SerializeField] private FloatingText _floatingTextPrefab;
        [SerializeField] private Transform _floatingTextRoot;
        [SerializeField] private TapParticle _tapParticlePrefab;
        [SerializeField] private WeatherView _weatherView;
        [SerializeField] private BreedsView _breedsView;
        [SerializeField] private BreedListItem _breedListItemPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NavigationController>().AsSingle();

            Container.Bind<ClickerConfig>().FromInstance(_clickerConfig).AsSingle();
            Container.Bind<CurrencyModel>().AsSingle();
            Container.Bind<EnergyModel>().AsSingle();
            Container.Bind<ClickerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AutoCollectService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyRegenService>().AsSingle();
            Container.Bind<IClickerView>().FromInstance(_clickerView).AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerPresenter>().AsSingle();

            Container.BindMemoryPool<FloatingText, FloatingText.Pool>()
                .WithInitialSize(8).FromComponentInNewPrefab(_floatingTextPrefab).UnderTransform(_floatingTextRoot);
            Container.BindMemoryPool<TapParticle, TapParticle.Pool>()
                .WithInitialSize(4).FromComponentInNewPrefab(_tapParticlePrefab).UnderTransformGroup("TapParticles");

            Container.Bind<IWeatherView>().FromInstance(_weatherView).AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherPresenter>().AsSingle();

            Container.Bind<IBreedsView>().FromInstance(_breedsView).AsSingle();
            Container.BindInterfacesAndSelfTo<BreedsPresenter>().AsSingle();
            Container.BindMemoryPool<BreedListItem, BreedListItem.Pool>()
                .WithInitialSize(10).FromComponentInNewPrefab(_breedListItemPrefab).UnderTransformGroup("BreedItems");
        }
    }
}

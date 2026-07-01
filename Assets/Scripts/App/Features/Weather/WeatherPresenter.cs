using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RequestQueueDemo.App.Navigation;
using RequestQueueDemo.Core.Network;
using RequestQueueDemo.Core.Network.Operations;
using RequestQueueDemo.Core.Queue;
using UnityEngine;

namespace RequestQueueDemo.App.Features.Weather
{
    public sealed class WeatherPresenter : ITab, IDisposable
    {
        private readonly IWeatherView _view;
        private readonly IRequestQueue _queue;
        private readonly IWebRequestRunner _runner;
        private readonly IApiConfig _config;
        private readonly ITextureCache _textureCache;
        private CancellationTokenSource _cts;

        public TabId Id => TabId.Weather;

        public WeatherPresenter(IWeatherView view, IRequestQueue queue, IWebRequestRunner runner,
                                IApiConfig config, ITextureCache textureCache)
        {
            _view = view; _queue = queue; _runner = runner; _config = config; _textureCache = textureCache;
        }

        public void OnEnter()
        {
            _view.Show();
            _cts = new CancellationTokenSource();
            PollLoop(_cts.Token).Forget();
        }

        public void OnExit()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _view.Hide();
        }

        private async UniTaskVoid PollLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var info = await _queue.EnqueueAsync(new WeatherRequestOperation(_runner, _config), ct);
                    _view.SetWeather(info.Display);
                    await LoadIconAsync(info.IconUrl, ct);
                }
                catch (OperationCanceledException) { return; }
                catch (Exception e) { Debug.LogWarning($"[Weather] {e.Message}"); }

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_config.WeatherPollIntervalSeconds),
                                        cancellationToken: ct);
                }
                catch (OperationCanceledException) { return; }
            }
        }

        private async UniTask LoadIconAsync(string url, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(url)) { _view.SetIconFallback(); return; }
            if (_textureCache.TryGet(url, out var cached)) { _view.SetIcon(cached); return; }
            try
            {
                var tex = await _queue.EnqueueAsync(new TextureRequestOperation(_runner, url), ct);
                _textureCache.Set(url, tex);
                _view.SetIcon(tex);
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception e)
            {
                Debug.LogWarning($"[Weather icon] {e.Message}");
                _view.SetIconFallback();
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}

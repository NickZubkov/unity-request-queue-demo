using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RequestQueueDemo.App.Navigation;
using RequestQueueDemo.Core.Network;
using RequestQueueDemo.Core.Network.Operations;
using RequestQueueDemo.Core.Queue;
using UnityEngine;

namespace RequestQueueDemo.App.Features.Breeds
{
    public sealed class BreedsPresenter : ITab, IDisposable
    {
        private readonly IBreedsView _view;
        private readonly IRequestQueue _queue;
        private readonly IWebRequestRunner _runner;
        private readonly IApiConfig _config;

        private CancellationTokenSource _cts;
        private CancellationTokenSource _itemCts;

        public TabId Id => TabId.Breeds;

        public BreedsPresenter(IBreedsView view, IRequestQueue queue, IWebRequestRunner runner, IApiConfig config)
        {
            _view = view; _queue = queue; _runner = runner; _config = config;
        }

        public void OnEnter()
        {
            _view.Show();
            _cts = new CancellationTokenSource();
            LoadBreedsAsync(_cts.Token).Forget();
        }

        public void OnExit()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _itemCts?.Dispose();
            _itemCts = null;
            _view.HideListLoader();
            _view.Hide();
        }

        private async UniTaskVoid LoadBreedsAsync(CancellationToken ct)
        {
            _view.ShowListLoader();
            try
            {
                var breeds = await _queue.EnqueueAsync(new BreedsListRequestOperation(_runner, _config), ct);
                _view.RenderBreeds(breeds, OnBreedClicked);
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { Debug.LogWarning($"[Breeds] {e.Message}"); }
            finally { _view.HideListLoader(); }
        }

        private void OnBreedClicked(string breedId)
        {
            _itemCts?.Cancel();
            _itemCts?.Dispose();
            _itemCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
            LoadFactsAsync(breedId, _itemCts.Token).Forget();
        }

        private async UniTaskVoid LoadFactsAsync(string breedId, CancellationToken ct)
        {
            _view.ShowFactsLoader(breedId);
            try
            {
                var facts = await _queue.EnqueueAsync(new BreedFactsRequestOperation(_runner, _config, breedId), ct);
                _view.ShowPopup(facts.Name, facts.Description);
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { Debug.LogWarning($"[Breed facts] {e.Message}"); }
            finally { _view.HideFactsLoader(breedId); }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _itemCts?.Dispose();
        }
    }
}

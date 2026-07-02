using System;
using System.Collections.Generic;
using RequestQueueDemo.App.Common.Ui;
using RequestQueueDemo.Core.Domain;
using UnityEngine;
using Zenject;

namespace RequestQueueDemo.App.Features.Breeds
{
    public sealed class BreedsView : MonoBehaviour, IBreedsView
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Transform _listRoot;
        [SerializeField] private LoaderView _listLoader;
        [SerializeField] private PopupView _popup;

        private BreedListItem.Pool _itemPool;
        private readonly List<BreedListItem> _spawned = new();
        private BreedListItem _loadingItem;

        [Inject]
        public void Construct(BreedListItem.Pool itemPool) => _itemPool = itemPool;

        public void Show() => _panel.SetActive(true);

        public void Hide()
        {
            ClearList();
            _popup.Hide();
            _panel.SetActive(false);
        }

        public void ShowListLoader() => _listLoader.Show();
        public void HideListLoader() => _listLoader.Hide();

        public void ShowFactsLoader(string breedId)
        {
            if (_loadingItem != null && _loadingItem.Id != breedId)
                _loadingItem.HideLoading();
            _loadingItem = FindItem(breedId);
            _loadingItem?.ShowLoading();
        }

        public void HideFactsLoader(string breedId)
        {
            if (_loadingItem != null && _loadingItem.Id == breedId)
                ClearLoading();
        }

        public void ShowPopup(string title, string body) => _popup.Show(title, body);

        public void RenderBreeds(IReadOnlyList<Breed> breeds, Action<string> onBreedClick)
        {
            ClearList();
            for (int i = 0; i < breeds.Count; i++)
            {
                var item = _itemPool.Spawn();
                item.transform.SetParent(_listRoot, false);
                item.Bind(i + 1, breeds[i].Id, breeds[i].Name, onBreedClick);
                _spawned.Add(item);
            }
        }

        private BreedListItem FindItem(string breedId)
        {
            foreach (var item in _spawned)
                if (item.Id == breedId) return item;
            return null;
        }

        private void ClearLoading()
        {
            if (_loadingItem == null) return;
            _loadingItem.HideLoading();
            _loadingItem = null;
        }

        private void ClearList()
        {
            ClearLoading();
            foreach (var item in _spawned) item.Despawn();
            _spawned.Clear();
        }
    }
}

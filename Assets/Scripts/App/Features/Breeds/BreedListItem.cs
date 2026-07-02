using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RequestQueueDemo.App.Features.Breeds
{
    public sealed class BreedListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _loadingImage;

        private Pool _pool;
        private string _id;
        private Action<string> _onClick;
        private Tween _spinTween;

        public string Id => _id;

        private void Awake() => _button.onClick.AddListener(() => _onClick?.Invoke(_id));

        public void Bind(int index, string id, string name, Action<string> onClick)
        {
            _id = id;
            _onClick = onClick;
            _label.text = $"{index} - {name}";
            HideLoading();
        }

        public void ShowLoading()
        {
            _spinTween?.Kill();
            _loadingImage.localRotation = Quaternion.identity;
            _loadingImage.gameObject.SetActive(true);
            _spinTween = _loadingImage
                .DOLocalRotate(new Vector3(0f, 0f, -360f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetLink(gameObject);
        }

        public void HideLoading()
        {
            _spinTween?.Kill();
            _spinTween = null;
            _loadingImage.localRotation = Quaternion.identity;
            _loadingImage.gameObject.SetActive(false);
        }

        public void Despawn()
        {
            HideLoading();
            _pool.Despawn(this);
        }

        public sealed class Pool : MonoMemoryPool<BreedListItem>
        {
            protected override void OnCreated(BreedListItem item)
            {
                base.OnCreated(item);
                item._pool = this;
            }
        }
    }
}

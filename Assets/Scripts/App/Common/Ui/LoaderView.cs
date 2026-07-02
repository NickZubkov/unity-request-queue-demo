using DG.Tweening;
using UnityEngine;

namespace RequestQueueDemo.App.Common.Ui
{
    public sealed class LoaderView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private RectTransform _loadingImage;

        private Tween _spinTween;

        public void Show()
        {
            _root.SetActive(true);
            StartSpin();
        }

        public void Hide()
        {
            StopSpin();
            _root.SetActive(false);
        }

        private void StartSpin()
        {
            if (_loadingImage == null) return;
            _spinTween?.Kill();
            _loadingImage.localRotation = Quaternion.identity;
            _spinTween = _loadingImage
                .DOLocalRotate(new Vector3(0f, 0f, -360f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        private void StopSpin()
        {
            _spinTween?.Kill();
            _spinTween = null;
        }
    }
}

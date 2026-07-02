using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RequestQueueDemo.App.Common.Ui
{
    public sealed class PopupView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _body;
        [SerializeField] private Button _closeButton;

        private void Awake() => _closeButton.onClick.AddListener(Hide);

        public void Show(string title, string body)
        {
            _title.text = title;
            _body.text = body;
            _root.SetActive(true);
            _group.DOKill();
            _group.alpha = 0f;
            _group.DOFade(1f, 0.2f).SetLink(gameObject);
        }

        public void Hide()
        {
            if (!_root.activeSelf) return;
            _group.DOKill();
            _group.DOFade(0f, 0.15f).OnComplete(() => _root.SetActive(false)).SetLink(gameObject);
        }
    }
}

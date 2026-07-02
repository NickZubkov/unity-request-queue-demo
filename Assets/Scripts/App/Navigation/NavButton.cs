using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;
using Zenject;

namespace RequestQueueDemo.App.Navigation
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public sealed class NavButton : MonoBehaviour
    {
        [SerializeField] private TabId _tabId;
        [SerializeField] private Color _activeColor = new(0.26f, 0.28f, 0.34f, 1f);
        [SerializeField] private Color _inactiveColor = new(0.18f, 0.19f, 0.24f, 1f);

        private NavigationController _navigation;
        private Image _background;
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        public void Construct(NavigationController navigation) => _navigation = navigation;

        private void Start()
        {
            _background = GetComponent<Image>();
            GetComponent<Button>().onClick.AddListener(OnClick);

            _navigation.Current
                .Subscribe(active => _background.color = active == _tabId ? _activeColor : _inactiveColor)
                .AddTo(_disposables);
        }

        private void OnClick()
        {
            _navigation.Switch(_tabId);
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void OnDestroy() => _disposables.Dispose();
    }
}

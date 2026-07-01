using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RequestQueueDemo.App.Navigation
{
    [RequireComponent(typeof(Button))]
    public sealed class NavButton : MonoBehaviour
    {
        [SerializeField] private TabId _tabId;
        private NavigationController _navigation;

        [Inject]
        public void Construct(NavigationController navigation) => _navigation = navigation;

        private void Awake() => GetComponent<Button>().onClick.AddListener(() => _navigation.Switch(_tabId));
    }
}

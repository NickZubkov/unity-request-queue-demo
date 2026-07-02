using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace RequestQueueDemo.App.Navigation
{
    public sealed class NavigationController : IInitializable
    {
        private readonly Dictionary<TabId, ITab> _tabs;
        private readonly ReactiveProperty<TabId> _current = new();
        private ITab _currentTab;

        public IReadOnlyReactiveProperty<TabId> Current => _current;

        public NavigationController(List<ITab> tabs)
        {
            _tabs = tabs.ToDictionary(t => t.Id);
        }

        public void Initialize()
        {
            foreach (var tab in _tabs.Values) tab.OnExit();
            Switch(TabId.Clicker);
        }

        public void Switch(TabId id)
        {
            if (_currentTab != null && _currentTab.Id == id) return;
            _currentTab?.OnExit();
            _currentTab = _tabs[id];
            _currentTab.OnEnter();
            _current.Value = id;
        }
    }
}

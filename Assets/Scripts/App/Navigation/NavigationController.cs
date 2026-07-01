using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace RequestQueueDemo.App.Navigation
{
    public sealed class NavigationController : IInitializable
    {
        private readonly Dictionary<TabId, ITab> _tabs;
        private ITab _current;

        public NavigationController(List<ITab> tabs)
        {
            _tabs = tabs.ToDictionary(t => t.Id);
        }

        public void Initialize() => Switch(TabId.Clicker);

        public void Switch(TabId id)
        {
            if (_current != null && _current.Id == id) return;
            _current?.OnExit();
            _current = _tabs[id];
            _current.OnEnter();
        }
    }
}

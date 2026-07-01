using UnityEngine;

namespace RequestQueueDemo.App.Navigation
{
    // Временная заглушка вкладки (удаляется по мере появления реальных презентеров).
    public sealed class StubTab : ITab
    {
        private readonly GameObject _panel;
        public TabId Id { get; }

        public StubTab(TabId id, GameObject panel)
        {
            Id = id;
            _panel = panel;
        }

        public void OnEnter() => _panel.SetActive(true);
        public void OnExit() => _panel.SetActive(false);
    }
}

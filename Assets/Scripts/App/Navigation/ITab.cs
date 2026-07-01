namespace RequestQueueDemo.App.Navigation
{
    // Реализует презентер вкладки. Показ/скрытие панели делегируется View.
    public interface ITab
    {
        public TabId Id { get; }
        public void OnEnter();
        public void OnExit();
    }
}

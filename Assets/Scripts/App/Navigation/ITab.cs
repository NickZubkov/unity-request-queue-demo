namespace RequestQueueDemo.App.Navigation
{
    // Реализует презентер вкладки. Показ/скрытие панели делегируется View.
    public interface ITab
    {
        TabId Id { get; }
        void OnEnter();
        void OnExit();
    }
}

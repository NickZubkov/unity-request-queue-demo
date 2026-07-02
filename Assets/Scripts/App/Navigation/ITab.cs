namespace RequestQueueDemo.App.Navigation
{
    public interface ITab
    {
        public TabId Id { get; }
        public void OnEnter();
        public void OnExit();
    }
}

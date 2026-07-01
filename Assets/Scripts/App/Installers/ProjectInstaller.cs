using Zenject;

namespace RequestQueueDemo.App.Installers
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Биндинги project-scope добавляются на этапах 1–2.
        }
    }
}

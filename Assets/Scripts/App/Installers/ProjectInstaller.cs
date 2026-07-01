using RequestQueueDemo.Core.Queue;
using Zenject;

namespace RequestQueueDemo.App.Installers
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RequestQueue>().AsSingle();
        }
    }
}

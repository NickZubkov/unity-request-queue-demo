using RequestQueueDemo.App.Config;
using RequestQueueDemo.Core.Network;
using RequestQueueDemo.Core.Queue;
using UnityEngine;
using Zenject;

namespace RequestQueueDemo.App.Installers
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ApiConfig _apiConfig;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RequestQueue>().AsSingle();
            Container.Bind<IApiConfig>().FromInstance(_apiConfig).AsSingle();
            Container.Bind<IWebRequestRunner>().To<UnityWebRequestRunner>().AsSingle();
            Container.Bind<ITextureCache>().To<TextureCache>().AsSingle();
        }
    }
}

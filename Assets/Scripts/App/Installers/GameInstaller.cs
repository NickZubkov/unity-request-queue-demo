using RequestQueueDemo.App.Navigation;
using UnityEngine;
using Zenject;

namespace RequestQueueDemo.App.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _clickerPanel;
        [SerializeField] private GameObject _weatherPanel;
        [SerializeField] private GameObject _breedsPanel;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NavigationController>().AsSingle();

            // Временные заглушки вкладок (заменяются реальными презентерами на этапах 3–5).
            Container.Bind<ITab>().FromInstance(new StubTab(TabId.Clicker, _clickerPanel)).AsCached();
            Container.Bind<ITab>().FromInstance(new StubTab(TabId.Weather, _weatherPanel)).AsCached();
            Container.Bind<ITab>().FromInstance(new StubTab(TabId.Breeds, _breedsPanel)).AsCached();
        }
    }
}

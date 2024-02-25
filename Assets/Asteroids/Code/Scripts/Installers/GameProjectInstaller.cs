using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class GameProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("Ran install bindings");
            //Container.BindInterfacesAndSelfTo<AddressablesManager>().FromComponentInHierarchy().AsSingle();
            //Container.BindInterfacesAndSelfTo<AddressablesManager>().FromComponentInHierarchy().AsSingle();
            GameSignalsInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            // GameSignalsInstaller.Install(Container);

            // Container.Bind<AssetLoader>().AsSingle();
            // Container.Bind<AssetBinder>().AsSingle();
            // Container.BindInterfacesAndSelfTo<BootManager>().AsSingle();
        }
    }   
}

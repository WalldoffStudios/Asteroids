using Zenject;

namespace Asteroids
{
    public class BootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AssetLoader>().AsSingle();
            Container.BindInterfacesTo<AssetBinder>().AsSingle();
            Container.BindInterfacesAndSelfTo<BootManager>().FromComponentOn(gameObject).AsSingle();
        }
    }
}

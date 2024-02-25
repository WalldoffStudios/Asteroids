using Zenject;

namespace Asteroids
{
    public class GameProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameSignalsInstaller.Install(Container);
            Container.BindInterfacesTo<SceneHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        }
    }   
}

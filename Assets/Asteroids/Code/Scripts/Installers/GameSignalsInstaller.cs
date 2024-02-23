using Zenject;

namespace Asteroids
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            //Input signals
            Container.DeclareSignal<MovementStateChangedSignal>();
            Container.DeclareSignal<MovementUpdateSignal>();
            Container.DeclareSignal<CameraZoomSignal>();
            Container.DeclareSignal<ShootInputChangedSignal>();

            //Status signals
            Container.DeclareSignal<PlayerHealthStatusChangedSignal>();
            Container.DeclareSignal<PlayerHealthInitializedSignal>();
            Container.DeclareSignal<ObjectDestroyedSignal>();
        }
    }   
}
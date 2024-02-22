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

            //Obstacle signals
            Container.DeclareSignal<ObstacleDestroyed>();
        }
    }   
}
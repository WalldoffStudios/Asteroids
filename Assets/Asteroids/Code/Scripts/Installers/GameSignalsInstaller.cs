using Zenject;

namespace Asteroids
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<MovementStateChangedSignal>();
            Container.DeclareSignal<MovementUpdateSignal>();
        }
    }   
}
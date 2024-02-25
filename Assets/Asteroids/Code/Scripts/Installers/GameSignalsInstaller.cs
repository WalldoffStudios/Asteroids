using Zenject;

namespace Asteroids
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            //Scene signals
            Container.DeclareSignal<TriggerSceneChangeSignal>();
            Container.DeclareSignal<SceneLoadCompletedSignal>();
            Container.DeclareSignal<GameSceneInitializedSignal>();
            
            //GameManager signals
            Container.DeclareSignal<GameStateChangedSignal>();

            //Input signals
            Container.DeclareSignal<MovementStateChangedSignal>();
            Container.DeclareSignal<MovementUpdateSignal>();
            Container.DeclareSignal<CameraZoomSignal>();
            Container.DeclareSignal<ShootInputChangedSignal>();
            Container.DeclareSignal<AimInputSignal>();

            //Status signals
            Container.DeclareSignal<PlayerHealthStatusChangedSignal>();
            Container.DeclareSignal<PlayerHealthInitializedSignal>();
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<ObjectDestroyedSignal>();
            
            //Currency signals
            Container.DeclareSignal<CurrencyAmountChangedSignal>();
            Container.DeclareSignal<CurrencySpawnSignal>();
        }
    }   
}
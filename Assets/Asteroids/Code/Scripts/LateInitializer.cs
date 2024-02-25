using Zenject;

namespace Asteroids
{
    public class LateInitializer : IInitializable
    {
        private readonly SignalBus _signalBus;

        public LateInitializer(SignalBus signalBus) => _signalBus = signalBus;
        
        public void Initialize()
        {
            _signalBus.Fire<GameSceneInitializedSignal>();
        }
    }   
}

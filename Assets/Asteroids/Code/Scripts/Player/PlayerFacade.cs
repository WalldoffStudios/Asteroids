using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerFacade : MonoBehaviour, IDamageAble
    {
        private Player _player;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(Player player, SignalBus signalBus)
        {
            _player = player;
            _signalBus = signalBus;
        }
        
        public bool IsDead => _player.IsDead;
        
        public float Rotation => _player.Rotation;

        private void Update()
        {
            if(IsDead == true) return;
            
            Vector3 localEulers = transform.localEulerAngles;
            localEulers.z = Rotation;
            transform.rotation = Quaternion.Euler(localEulers);
        }

        public void TakeDamage(int amount)
        {
            _signalBus.Fire(new PlayerHealthStatusChangedSignal(-amount));
        }
    }   
}

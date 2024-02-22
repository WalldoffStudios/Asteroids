using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerWeaponHandler : IInitializable, IDisposable, ITickable
    {
        [Serializable]
        public class Settings
        {
            public float cooldownDelay;
        }
        
        public IWeapon EquippedWeapon { get; private set; }
        private readonly Player _player;
        private readonly Settings _settings;
        private readonly SignalBus _signalBus;

        public PlayerWeaponHandler(IWeapon weapon, Player player, Settings settings, SignalBus signalBus)
        {
            EquippedWeapon = weapon;
            _player = player;
            _settings = settings;
            _signalBus = signalBus;
        }

        private float _ticksSinceLastShot;
        private bool _isFiring;

        public void Initialize()
        {
            _signalBus.Subscribe<ShootInputChangedSignal>(ShootInputChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ShootInputChangedSignal>(ShootInputChanged);
        }

        private void ShootInputChanged(ShootInputChangedSignal signal)
        {
            _isFiring = signal.StartedShooting;
        }

        public void Tick()
        {
            _ticksSinceLastShot += Time.deltaTime;
            if (_ticksSinceLastShot > _settings.cooldownDelay && _isFiring)
            {
                EquippedWeapon.Fire();
                _ticksSinceLastShot = 0.0f;
            }
        }
    }   
}

using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerRotationHandler : IInitializable, IDisposable
    {
        private readonly Player _player;
        private readonly SignalBus _signalBus;
        private readonly Camera _camera;

        public PlayerRotationHandler(Player player, SignalBus signalBus, Camera camera)
        {
            _player = player;
            _signalBus = signalBus;
            _camera = camera;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<AimInputSignal>(AimInput);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<AimInputSignal>(AimInput);
        }

        private void AimInput(AimInputSignal signal)
        {
            Vector2 mouseWorldPosition = _camera.ScreenToWorldPoint(signal.AimPosition);

            Vector2 direction = (mouseWorldPosition - _player.Position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            
            _player.Rotation = angle;
        }
    }   
}

using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerMoveHandler : IInitializable, IDisposable, IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            public float MoveSpeed;
            public float MaxSpeed;
        }
        
        private Vector2 _moveDirection;
        private bool _isMoving;

        private readonly Settings _settings;
        private readonly Player _player;
        private readonly SignalBus _signalBus;
        
        public PlayerMoveHandler(Player player, Settings settings, SignalBus signalBus)
        {
            _player = player;
            _settings = settings;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<MovementUpdateSignal>(MovePlayer);
            _signalBus.Subscribe<MovementStateChangedSignal>(MovementStateChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<MovementUpdateSignal>(MovePlayer);
            _signalBus.Unsubscribe<MovementStateChangedSignal>(MovementStateChanged);
        }

        private void MovePlayer(MovementUpdateSignal signal)
        {
            if(_player.IsDead == true) return;
            _moveDirection = signal.Direction;
        }

        private void MovementStateChanged(MovementStateChangedSignal signal)
        {
            _isMoving = signal.MovementStarted;
        }

        public void FixedTick()
        {
            if(_player.IsDead == true) return;
            if(_isMoving == false) return;
            _player.AddForce(_moveDirection * _settings.MoveSpeed);
        }
    }   
}

using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerMoveHandler : IInitializable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            public float MoveSpeed;
            public float MaxSpeed;
        }

        private readonly ScreenBorders _screenBorders;
        private readonly Settings _settings;
        private readonly Player _player;
        private readonly SignalBus _signalBus;

        public PlayerMoveHandler(
             ScreenBorders borders,
            Player player, 
            Settings settings, 
             SignalBus signalBus)
        {
            _screenBorders = borders;
            _player = player;
            _settings = settings;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<MovementUpdateSignal>(MovePlayer);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<MovementUpdateSignal>(MovePlayer);
        }

        private void MovePlayer(MovementUpdateSignal signal)
        {
            if(_player.IsDead == true) return;
            _player.AddForce(signal.Direction * _settings.MoveSpeed);
        }

        // public void FixedTick()
        // {
        //     if(_player.IsDead == true) return;
        //     
        //     if (_player.Velocity.sqrMagnitude > 0.0f)
        //     {
        //         if (_screenBorders.IsNearEdge(_player.Position) == true)
        //         {
        //             Vector2 direction = _screenBorders.GetBounceDirection(_player.Position, _player.Velocity);
        //             _player.Bounce(direction);
        //         }
        //     }
        // }
    }   
}

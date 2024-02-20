using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerMoveHandler : IFixedTickable
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
        private readonly PlayerInputState _inputState;

        public PlayerMoveHandler(
             ScreenBorders borders,
            Player player, 
            Settings settings, 
            PlayerInputState inputState)
        {
            _screenBorders = borders;
            _player = player;
            _settings = settings;
            _inputState = inputState;
        }

        public void FixedTick()
        {
            if(_player.IsDead == true) return;
            
            if (_inputState.IsMoving == true)
            {
                _player.AddForce(_inputState.MovementInput * _settings.MoveSpeed);
            }
            if (_player.Velocity.sqrMagnitude > 0.0f)
            {
                if (_screenBorders.IsNearEdge(_player.Position) == true)
                {
                    Vector2 direction = _screenBorders.GetBounceDirection(_player.Position, _player.Velocity);
                    _player.Bounce(direction);
                }
            }
        }
    }   
}

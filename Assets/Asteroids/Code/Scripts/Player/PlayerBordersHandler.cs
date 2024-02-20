using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerBordersHandler : ITickable
    {
        [Serializable]
        public class Settings
        {
            public bool bounceOnBorders;
        }
        
        private readonly Player _player;
        private readonly ScreenBorders _screenBorders;
        private readonly Settings _settings;

        public PlayerBordersHandler(Player player, ScreenBorders borders, Settings settings)
        {
            _player = player;
            _screenBorders = borders;
            _settings = settings;
        }

        public void Tick()
        {
            if(_player.IsDead == true) return;
            if (_player.Velocity.sqrMagnitude > 0.0f == false) return;
            if (_screenBorders.IsNearEdge(_player.Position) == false) return;
            
            if (_settings.bounceOnBorders == true)
            {
                Vector2 direction = _screenBorders.GetBounceDirection(_player.Position, _player.Velocity);
                _player.Bounce(direction);   
            }
            else //if false we will teleport player
            {
                Vector2 teleportPosition = _screenBorders.GetTeleportPosition(_player.Position);
                _player.Position = teleportPosition;
            }
        }
    }   
}

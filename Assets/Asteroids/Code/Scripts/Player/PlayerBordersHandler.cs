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
            Vector2 playerPos = _player.Position;
            bool insideScreenBounds = _screenBorders.IsInsideScreenBounds(playerPos);

            if (insideScreenBounds == true && _settings.bounceOnBorders == true)
            {
                if (_screenBorders.IsNearInsideEdge(_player.Position) == true)
                {
                    Vector2 direction = _screenBorders.GetBounceDirection(_player.Position, _player.Velocity);
                    _player.Bounce(direction);    
                }
            }
            else if (insideScreenBounds == false && _settings.bounceOnBorders == false)
            {
                Vector2 teleportPosition = _screenBorders.GetTeleportPosition(_player.Position, _player.Velocity, 0.0f);
                _player.Position = teleportPosition;
            }
        }
    }   
}

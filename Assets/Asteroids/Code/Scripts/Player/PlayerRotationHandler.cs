using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerRotationHandler : ITickable
    {
        [Serializable]
        public class Settings
        {
            public float RotationSpeed;
        }

        private readonly Player _player;
        private readonly Settings _settings;
        private readonly PlayerInputState _inputState;

        public PlayerRotationHandler(Player player, Settings settings, PlayerInputState inputState)
        {
            _player = player;
            _settings = settings;
            _inputState = inputState;
        }
        
        public void Tick()
        {
            float angle = Mathf.Atan2(_player.Velocity.y, _player.Velocity.x) * Mathf.Rad2Deg - 45.0f;
            _player.Rotation = angle;
        }
    }   
}

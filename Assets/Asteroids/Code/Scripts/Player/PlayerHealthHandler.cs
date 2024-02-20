using System;
using UnityEngine;

namespace Asteroids
{
    public class PlayerHealthHandler
    {
        [Serializable]
        public class Settings
        {
            public float MaxHealth;
        }

        private readonly Player _player;
        private readonly Settings _settings;

        public PlayerHealthHandler(Player player, Settings settings)
        {
            _player = player;
            _settings = settings;
            Debug.Log(_settings.MaxHealth);
        }

        public void IncreaseHealth(int amount)
        {
            //todo: add health
        }

        public void DecreaseHealth(int amount)
        {
            //todo: remove health
        }
    }   
}
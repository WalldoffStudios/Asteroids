using System;

namespace Asteroids
{
    public class PlayerHealthHandler
    {
        [Serializable]
        public class Settings
        {
            public float HealthLoss;
            public float HitForce;
        }

        private readonly Player _player;
        private readonly Settings _settings;

        public PlayerHealthHandler(Player player, Settings settings)
        {
            _player = player;
            _settings = settings;
        }

        public void IncreaseHealth(float amount)
        {
            //todo: add health
        }

        public void DecreaseHealth(int amount)
        {
            //todo: remove health
        }
    }   
}
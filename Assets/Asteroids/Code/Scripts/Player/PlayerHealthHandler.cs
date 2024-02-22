using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerHealthHandler : IInitializable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            public int maxHealth;
        }

        private readonly Player _player;
        private readonly SignalBus _signalBus;
        private readonly Settings _settings;

        public PlayerHealthHandler(Player player, SignalBus signalBus, Settings settings)
        {
            _player = player;
            _signalBus = signalBus;
            _settings = settings;
        }

        private int _currentHealth;
        
        public void Initialize()
        {
            _currentHealth = _settings.maxHealth;
            _signalBus.Subscribe<PlayerHealthStatusChanged>(HandleHealthChange);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerHealthStatusChanged>(HandleHealthChange);
        }

        private void HandleHealthChange(PlayerHealthStatusChanged signal)
        {
            int amount = signal.HealthAmount;
            if (amount > 0)
            {
                IncreaseHealth(amount);
            }
            else
            {
                DecreaseHealth(amount);
            }
            
            Debug.Log($"Player health was updated, new health is {_currentHealth}");
        }

        private void IncreaseHealth(int amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _settings.maxHealth);
        }

        private void DecreaseHealth(int amount)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - amount);
            if(_currentHealth == 0) _player.Died();
        }
    }   
}
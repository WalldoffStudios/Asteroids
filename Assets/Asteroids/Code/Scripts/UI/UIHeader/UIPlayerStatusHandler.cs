using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UIPlayerStatusHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly GameObject _healthContainer;
        private readonly TextMeshProUGUI _healthText;
        private readonly RectTransform _healthBarRect;

        public UIPlayerStatusHandler(UIHeaderCanvas headerCanvas, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _healthContainer = headerCanvas.HealthContainer;
            _healthText = headerCanvas.HealthText;
            _healthBarRect = headerCanvas.HealthRect;
        }

        private int _maxHealth;
        private int _currentHealth;
        
        public void Initialize()
        {
            _signalBus.Subscribe<PlayerHealthInitializedSignal>(HealthInitialized);
            _signalBus.Subscribe<PlayerHealthStatusChangedSignal>(HealthStatusChanged);
            _healthContainer.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerHealthStatusChangedSignal>(HealthStatusChanged);
            _signalBus.Unsubscribe<PlayerHealthInitializedSignal>(HealthInitialized);
        }

        private void HealthInitialized(PlayerHealthInitializedSignal signal)
        {
            _maxHealth = Mathf.Max(1, signal.MaxHealth);
            _currentHealth = _maxHealth;
            _healthContainer.gameObject.SetActive(true);
            UpdateHealthUI();
        }

        private void HealthStatusChanged(PlayerHealthStatusChangedSignal signal)
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
            
            UpdateHealthUI();
        }
        
        private void IncreaseHealth(int amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        }

        //amount is a negative value here
        private void DecreaseHealth(int amount)
        {
            _currentHealth = Mathf.Max(0, _currentHealth + amount);
        }

        private void UpdateHealthUI()
        {
            Vector2 healthBarSize = _healthBarRect.localScale;
            if (_currentHealth > 0)
            {
                _healthText.text = $"{_currentHealth}/{_maxHealth}";
                healthBarSize.x = (float)_currentHealth / _maxHealth;
            }
            else
            {
                _healthText.text = "Dead";
                healthBarSize.x = 0.0f;
            }

            _healthBarRect.localScale = healthBarSize;
        }
    }   
}
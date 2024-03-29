using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids
{
    public class UICurrencyHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly TextMeshProUGUI _currencyText;
        private readonly RectTransform _currencyImageRect;

        public UICurrencyHandler(
            UIHeaderCanvas headerCanvas,
            SignalBus signalBus,
            CoinTextureReference coinTextureReference)
        {
            _signalBus = signalBus;
            _currencyText = headerCanvas.CurrencyText;
            _currencyImageRect = headerCanvas.CurrencyImageRect;
            Image currencyImage = headerCanvas.CurrencyImage;
            currencyImage.sprite = coinTextureReference.CoinTexture;
        }

        private int _currentCurrencyCount;
        
        public void Initialize()
        {
            _signalBus.Subscribe<CurrencyAmountChangedSignal>(CurrencyAmountChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<CurrencyAmountChangedSignal>(CurrencyAmountChanged);
        }

        private void CurrencyAmountChanged(CurrencyAmountChangedSignal signal)
        {
            _currentCurrencyCount += signal.CurrencyAmount;
            UpdateCurrencyText();
        }

        private void UpdateCurrencyText()
        {
            _currencyText.text = $"{_currentCurrencyCount}";
            MonoBehaviourHelper.Instance.TweenScale(_currencyImageRect, 1.0f, 1.25f, 0.2f);
        }
    }   
}
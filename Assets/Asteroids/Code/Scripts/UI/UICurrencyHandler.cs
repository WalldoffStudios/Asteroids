using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UICurrencyHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly TextMeshProUGUI _currencyText;
        private readonly RectTransform _currencyImageRect;

        public UICurrencyHandler(UIHeaderCanvas headerCanvas, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _currencyText = headerCanvas.CurrencyText;
            _currencyImageRect = headerCanvas.CurrencyImageRect;
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
            ScaleUpIcon();
        }
        
        private void ScaleUpIcon()
        {
            DOTween.Sequence().Append(_currencyImageRect
                    .DOScale(Vector3.one * 1.25f, 0.2f)
                    .SetEase(Ease.InQuad))
                .OnComplete(ScaleDownIcon);
        }

        private void ScaleDownIcon()
        {
            DOTween.Sequence().Append(_currencyImageRect
                    .DOScale(Vector3.one, 0.2f)
                    .SetEase(Ease.OutQuad));
        }
    }   
}
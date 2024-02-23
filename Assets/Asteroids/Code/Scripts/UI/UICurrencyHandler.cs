using System;
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
        
        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }   
}
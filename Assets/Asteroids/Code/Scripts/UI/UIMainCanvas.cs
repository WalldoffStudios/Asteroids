using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UIMainCanvas : IInitializable, IDisposable
    {
        private readonly RectTransform _mainCanvasRect;
        private readonly SignalBus _signalBus;

        public UIMainCanvas(RectTransform mainCanvasRect, SignalBus signalBus)
        {
            _mainCanvasRect = mainCanvasRect;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(GameStateChanged);
        }

        public void Dispose()
        {
            _mainCanvasRect.gameObject.SetActive(false);
        }
        
        private void GameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.State == GameStates.WaitingToStart)
            {
                _mainCanvasRect.gameObject.SetActive(true);
            }
        }
    }   
}

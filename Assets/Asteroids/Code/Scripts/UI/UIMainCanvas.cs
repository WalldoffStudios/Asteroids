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
            _mainCanvasRect.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(GameStateChanged);
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

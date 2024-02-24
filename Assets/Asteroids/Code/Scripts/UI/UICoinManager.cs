using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class UICoinManager : IInitializable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            public float minPopupSpeed;
            public float maxPopupSpeed;
            public float cornerXPadding;
            public float cornerYPadding;
        }

        private readonly RectTransform _mainCanvasRect;
        private readonly Camera _mainCamera;
        private readonly UICoin.Factory _coinFactory;
        private readonly SignalBus _signalBus;
        private readonly Settings _settings;

        public UICoinManager(RectTransform mainCanvasRect,
            Camera mainCamera,
            UICoin.Factory coinFactory,
            SignalBus signalBus,
            Settings settings)
        {
            _mainCanvasRect = mainCanvasRect;
            _mainCamera = mainCamera;
            _coinFactory = coinFactory;
            _signalBus = signalBus;
            _settings = settings;
        }

        private Vector2 _coinCollectTarget;
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(GameStateChanged);
            _signalBus.Subscribe<CurrencySpawnSignal>(CurrencySpawn);
            
            Vector2 topRightScreenPosition = new Vector2(Screen.width, Screen.height);
            Vector2 topRightPos = _mainCanvasRect.InverseTransformPoint(topRightScreenPosition);
            topRightPos.x -= _settings.cornerXPadding;
            topRightPos.y -= _settings.cornerYPadding;
            _coinCollectTarget = topRightPos;
            
            _mainCanvasRect.gameObject.SetActive(false);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<CurrencySpawnSignal>(CurrencySpawn);
        }

        private void GameStateChanged(GameStateChangedSignal signal)
        {
            
        }

        private void CurrencySpawn(CurrencySpawnSignal signal)
        {
            int coinsToSpawn = signal.AmountToSpawn;
            for (int i = 0; i < coinsToSpawn; i++)
            {
                float popupSpeed = Random.Range(_settings.minPopupSpeed, _settings.maxPopupSpeed);
                float collectSpeed = Random.Range(_settings.minPopupSpeed, _settings.maxPopupSpeed);
                CoinSpawnParams spawnParams = new CoinSpawnParams(popupSpeed, collectSpeed, _coinCollectTarget );
                UICoin coin = _coinFactory.Create(spawnParams);
                coin.transform.position = ConvertToUISpace(signal.Position);
                coin.PopupCoin();   
            }
        }

        private Vector2 ConvertToUISpace(Vector2 pos)
        {
            return _mainCamera.WorldToScreenPoint(pos);
        }
    }   
}

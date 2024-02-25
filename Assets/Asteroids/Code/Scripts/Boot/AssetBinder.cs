using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Asteroids
{
    public class AssetBinder : IInitializable, IDisposable
    {
        private readonly DiContainer _container;
        private readonly SignalBus _signalBus;

        public AssetBinder(DiContainer container, SignalBus signalBus)
        {
            _container = container;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            Debug.Log("Subscribe to asset download signal");
            _signalBus.Subscribe<AssetsDownloadedSignal>(AssetsDownloaded);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<AssetsDownloadedSignal>(AssetsDownloaded);
        }

        private void AssetsDownloaded(AssetsDownloadedSignal signal)
        {
            Debug.Log("should bind assets");
            BootManager bootManager = signal.BootManager;
            if(bootManager.LoadedAssets.TryGetValue("Ship", out Object sprite))
            {
                if (sprite is Sprite value)
                {
                    PlayerDataNew playerData = new PlayerDataNew(value);
                    _container.BindInstance(playerData).IfNotBound();
                }
            }
            //todo: bind the stuff here by getting it from the bootmanager
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }

    public class PlayerDataNew
    {
        public Sprite PlayerSprite { get; private set; }
        public PlayerDataNew(Sprite playerSprite) => PlayerSprite = playerSprite;
    }
}

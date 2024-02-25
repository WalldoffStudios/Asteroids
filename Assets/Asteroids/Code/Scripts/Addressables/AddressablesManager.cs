using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Asteroids
{
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid): base(guid){}
    }
    
    public class AddressablesManager : MonoBehaviour, IInitializable
    {
        [Serializable]
        public class Settings
        {
            public AssetReferenceTexture2D PlayerTextureReference;
            public AssetReferenceTexture2D CoinTextureReference;
        }

        [SerializeField] private TextMeshProUGUI loadingStatusText = null;
        [SerializeField] private TextMeshProUGUI loadingPercentageText = null;

        private PlayerTextureReference _playerTextureReference;
        private CoinTextureReference _coinTextureReference;
        
        private SignalBus _signalBus;
        private Settings _settings;

        [Inject]
        public void Construct(SignalBus signalBus, Settings settings)
        {
            _signalBus = signalBus;
            _settings = settings;
        }
        
        public void Initialize()
        {
            StartCoroutine(LoadAssetsCoroutine());
        }

        private readonly int _totalDownloadOperations = 2;
        private IEnumerator LoadAssetsCoroutine()
        {
            int operationsSuccessful = 0;
            loadingStatusText.text = $"Downloading player texture";
            loadingPercentageText.text = $"0% / 0%";

            AsyncOperationHandle<Texture2D> playerTextureHandle = _settings.PlayerTextureReference.LoadAssetAsync<Texture2D>();
            yield return playerTextureHandle;
            if (playerTextureHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Texture2D playerTexture = playerTextureHandle.Result;
                Sprite newSprite = Sprite.Create(playerTexture, new Rect(0.0f, 0.0f, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                _playerTextureReference = new PlayerTextureReference(newSprite);
                operationsSuccessful++;
                loadingStatusText.text = $"Player texture downloaded";
                UpdateLoadStatusText(operationsSuccessful);
            }
            AsyncOperationHandle<Texture2D> coinTextureHandle = _settings.CoinTextureReference.LoadAssetAsync<Texture2D>();
            yield return coinTextureHandle;
            if (coinTextureHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Texture2D coinTexture = coinTextureHandle.Result;
                Sprite newSprite = Sprite.Create(coinTexture, new Rect(0.0f, 0.0f, coinTexture.width, coinTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                _coinTextureReference = new CoinTextureReference(newSprite);
                operationsSuccessful++;
                loadingStatusText.text = $"Coin texture downloaded";
                UpdateLoadStatusText(operationsSuccessful);
            }
            
        
            // Load the audio clip
            // AsyncOperationHandle<AudioClip> audioClipHandle = audioClipReference.LoadAssetAsync<AudioClip>();
            // yield return audioClipHandle;
            // if (audioClipHandle.Status == AsyncOperationStatus.Succeeded)
            // {
            //     AudioClip audioClip = audioClipHandle.Result;
            //     testingAudioSource.clip = audioClip;
            //     testingAudioSource.Play();
            //     operationsSuccessful++;
            //     Debug.Log($"Successfully loaded player audio clip with name {audioClip.name}");
            // }

            if (operationsSuccessful == _totalDownloadOperations)
            {
                OnAllAssetsLoaded();   
            }
        }

        private void UpdateLoadStatusText(int operationsSuccessful)
        {
            loadingPercentageText.text = $"{operationsSuccessful / _totalDownloadOperations} / 100%";
        }
        
        private void OnAllAssetsLoaded()
        {
            DiContainer projectContainer = ProjectContext.Instance.Container;
            projectContainer.BindInstance(_playerTextureReference).IfNotBound();
            projectContainer.BindInstance(_coinTextureReference).IfNotBound();
            
            _signalBus.Fire(new TriggerSceneChangeSignal(1));
            //_signalBus.Fire(new AssetsBoundSignal());
        }
    }   
}

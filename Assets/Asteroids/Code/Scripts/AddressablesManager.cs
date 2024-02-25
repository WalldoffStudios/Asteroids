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
        }

        [SerializeField] private TextMeshProUGUI loadingStatusText = null;
        [SerializeField] private TextMeshProUGUI loadingPercentageText = null;

        private PlayerTextureAsset _playerTextureAsset;
        
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

        private int totalDownloadOperations = 1;
        private IEnumerator LoadAssetsCoroutine()
        {
            int operationsSuccessful = 0;
            loadingStatusText.text = $"Downloading player texture";
            loadingPercentageText.text = $"0% / 0%";
            // AsyncOperationHandle<Texture2D> textureHandle = playerTextureReference.LoadAssetAsync<Texture2D>();
            // yield return textureHandle;
            // if (textureHandle.Status == AsyncOperationStatus.Succeeded)
            // {
            //     Texture2D playerTexture = textureHandle.Result;
            //     Sprite newSprite = Sprite.Create(playerTexture, new Rect(0.0f, 0.0f, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            //     testingRenderer.sprite = newSprite;
            //     operationsSuccessful++;
            //     Debug.Log($"Successfully loaded player texture with name {playerTexture.name}");
            // }
            AsyncOperationHandle<Texture2D> textureHandle = _settings.PlayerTextureReference.LoadAssetAsync<Texture2D>();
            yield return textureHandle;
            if (textureHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Texture2D playerTexture = textureHandle.Result;
                Sprite newSprite = Sprite.Create(playerTexture, new Rect(0.0f, 0.0f, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                _playerTextureAsset = new PlayerTextureAsset(newSprite);
                operationsSuccessful++;
                loadingStatusText.text = $"Player texture downloaded";
                loadingPercentageText.text = $"{operationsSuccessful / totalDownloadOperations} / 100%";
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

            if (operationsSuccessful == 1)
            {
                OnAllAssetsLoaded();   
            }
        }
        
        private void OnAllAssetsLoaded()
        {
            DiContainer projectContainer = ProjectContext.Instance.Container;
            projectContainer.BindInstance(_playerTextureAsset).IfNotBound();
            
            _signalBus.Fire(new TriggerSceneChangeSignal(1));
            //_signalBus.Fire(new AssetsBoundSignal());
        }
    }   
}

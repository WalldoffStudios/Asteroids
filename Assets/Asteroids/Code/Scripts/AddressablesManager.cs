using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroids
{
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid): base(guid){}
    }
    
    public class AddressablesManager : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceTexture2D playerTextureReference;
        
        [SerializeField] 
        private AssetReferenceAudioClip audioClipReference;
        
        [SerializeField] 
        private SpriteRenderer testingRenderer = null;
        
        [SerializeField] 
        private AudioSource testingAudioSource = null;
        

        private void Start()
        {
            // Addressables.WebRequestOverride = webRequest =>
            // {
            //     string token = "c43181454323c1ce21875e3747fbc29a";
            //     webRequest.SetRequestHeader("Authorization", "Bearer " + token);
            // };
            
            // Replace "your_token_here" with your actual encoded Bucket Access Token
            //string token = "f88822b7151a9d9e2e8b3beec30fa966";

            // Example of loading an asset with Addressables using the overridden web request
            // Addressables.LoadAssetAsync<TextAsset>("your_asset_address").Completed += handle =>
            // {
            //     if (handle.Status == AsyncOperationStatus.Succeeded)
            //     {
            //         // Use your loaded asset here
            //         Debug.Log(handle.Result.text);
            //     }
            // };
            
            // Debug.Log($"{EncodeToBase64("f88822b7151a9d9e2e8b3beec30fa966")}");
            // Debug.Log($"{EncodeToBase64("c43181454323c1ce21875e3747fbc29a")}");
            //Debug.Log($"{EncodeToBase64("c43181454323c1ce21875e3747fbc29a")}");
            
            // Start the coroutine to load assets
            StartCoroutine(LoadAssetsCoroutine());
        }
        
        public static string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            string encodedString = Convert.ToBase64String(toEncodeAsBytes);
            return encodedString;
        }

        private IEnumerator LoadAssetsCoroutine()
        {
            // Load the player texture
            AsyncOperationHandle<Texture2D> textureHandle = playerTextureReference.LoadAssetAsync<Texture2D>();
            yield return textureHandle;
            if (textureHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Texture2D playerTexture = textureHandle.Result;
                Sprite newSprite = Sprite.Create(playerTexture, new Rect(0.0f, 0.0f, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                testingRenderer.sprite = newSprite;
                //testingRenderer.material.mainTexture = playerTexture;
                Debug.Log($"Succesfully loaded player texture with name {playerTexture.name}");
                // Use the loaded player texture here
                // For example, apply it to a material or a UI element
            }
        
            // Load the audio clip
            AsyncOperationHandle<AudioClip> audioClipHandle = audioClipReference.LoadAssetAsync<AudioClip>();
            yield return audioClipHandle;
            if (audioClipHandle.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip audioClip = audioClipHandle.Result;
                testingAudioSource.clip = audioClip;
                testingAudioSource.Play();
                Debug.Log($"Succesfully loaded player texture with name {audioClip.name}");
                // Use the loaded audio clip here
                // For example, play the clip using an AudioSource
            }
        
            // After loading both assets, you can now indicate that everything is done.
            // For example, you can call another method here or trigger an event.
            OnAllAssetsLoaded();
        }
        
        private void OnAllAssetsLoaded()
        {
            // Notify that all assets are loaded successfully
            Debug.Log("All assets are loaded.");
        }
        
        private void OnDestroy()
        {
            playerTextureReference.ReleaseAsset();
            audioClipReference.ReleaseAsset();
        }
    }   
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroids
{
    public class AssetLoader
    {
        // public IEnumerator LoadAsset<T>(AssetReferenceT<T> assetReference) where T : Component
        // {
        //     var handle = assetReference.LoadAssetAsync<T>();
        //     yield return handle;
        // }
        
        public IEnumerator LoadAsset(AssetReference assetReference, Action<UnityEngine.Object> onAssetLoaded)
        {
            var handle = assetReference.LoadAssetAsync<UnityEngine.Object>(); // Load as a base Unity object
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                onAssetLoaded?.Invoke(handle.Result);
            }
            else
            {
                Debug.LogError($"Failed to load asset: {assetReference.RuntimeKey}");
            }
        }
    }   
}

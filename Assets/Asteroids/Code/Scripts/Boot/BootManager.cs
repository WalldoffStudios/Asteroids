using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace Asteroids
{
    [Serializable]
    public struct AssetRefAndKey
    {
        public AssetReference assetReference;
        public string assetKey;
    }

    // public class AssetsDownloadedSignal
    // {
    //     public BootManager BootManager { get; private set; }
    //     public AssetsDownloadedSignal(BootManager bootManager) => BootManager = bootManager;
    // }
    //
    // public class AssetsBoundSignal
    // { }
    
    public class BootManager : MonoBehaviour
    {
        //[SerializeField] private AssetReference[] assetsToLoad = null;
        // [SerializeField] private AssetRefAndKey[] assetAndKeyRefs = null;
        // [SerializeField] private ComponentAssetBindings[] componentBindings;
        private AssetRefAndKey[] _assetAndKeyRefs;

        private AssetLoader _assetLoader;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(AssetLoader loader, AssetRefAndKey[] assetRefAndKeys, SignalBus signalBus)
        {
            _assetLoader = loader;
            _assetAndKeyRefs = assetRefAndKeys;
            _signalBus = signalBus;
        }
        
        public Dictionary<string, Object> LoadedAssets { get; private set; }
        
        public void Start()
        {
            Debug.Log("initialized boot manager");
            LoadedAssets = new Dictionary<string, Object>();
            StartCoroutine(LoadAllAssets());
        }
        
        private IEnumerator LoadAllAssets()
        {
            int loadedCount = 0; // Track the number of successfully loaded assets

            foreach (var assetRef in _assetAndKeyRefs)
            {
                // Load each asset
                yield return _assetLoader.LoadAsset(assetRef.assetReference, loadedAsset =>
                {
                    if (loadedAsset != null)
                    {
                        //_loadedAssets.TryAdd(assetRef.assetKey, loadedAsset);
                        if (LoadedAssets.ContainsKey(assetRef.assetKey) == false)
                        {
                            loadedCount++;
                            LoadedAssets.Add(assetRef.assetKey, loadedAsset);
                            Debug.Log($"Added to dictionary, current count is {LoadedAssets.Count}");
                        }
                        //_loadedAssets[assetRef.assetKey] = loadedAsset;
                    }
                    else
                    {
                        Debug.LogError($"Failed to load asset with key: {assetRef.assetKey}");
                    }
                });
            }

            // Check if all assets are loaded
            if (loadedCount == _assetAndKeyRefs.Length)
            {
                // Proceed with binding data objects
                //BindDataObjects();
                Debug.Log("Fired assets downloaded signal");
                //_signalBus.Fire<AssetsDownloadedSignal>();
                _signalBus.Fire(new AssetsDownloadedSignal(this));
                //BindInstances();
            }
            else
            {
                Debug.LogError("Not all assets were loaded successfully.");
            }
        }

        // private void BindInstances()
        // {
        //     for (int i = 0; i < assetDatas.Length; i++)
        //     {
        //         Data assetData = assetDatas[i];
        //         assetData.InjectAssets(LoadedAssets);
        //         ProjectContext.Instance.Container.BindInstance(assetData);
        //         //ProjectContext.Instance.Container.BindInstance(assetData.GetType(), assetData).AsSingle();
        //         //ProjectContext.Instance.Container.Bind(assetData).To<assetData.GetType>().AsSingle();
        //     }
        // }

        // private void BindDataObjects()
        // {
        //     foreach (var componentBinding in _componentBindings)
        //     {
        //         List<Object> dataObjects = new List<Object>();
        //         for (int i = 0; i < componentBinding.assetKeys.Length; i++)
        //         {
        //             string key = componentBinding.assetKeys[i];
        //             if (LoadedAssets.TryGetValue(key, out Object data))
        //             {
        //                 dataObjects.Add(data);
        //             }
        //         }
        //         Debug.Log($"tried to bound {componentBinding.data.name} with objects length of {dataObjects.Count}");
        //         MethodInfo injectDataMethod = componentBinding.data.GetType().GetMethod("InjectData");
        //         injectDataMethod?.Invoke(componentBinding.data, new object[] { dataObjects.ToArray() });
        //         
        //         _assetBinder.Bind(componentBinding.data.GetType(), componentBinding.data);
        //         // componentBinding.data.InjectData(dataObjects.ToArray());
        //         // _assetBinder.Bind((PlayerData)componentBinding.data);
        //         //_assetBinder.Bind(typeof(PlayerData), componentBinding.data);
        //     }
        //
        //     SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        // }
    }   
}

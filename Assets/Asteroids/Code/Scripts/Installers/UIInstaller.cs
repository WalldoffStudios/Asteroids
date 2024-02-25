using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UIInstaller : MonoInstaller
    {
        [Serializable]
        public class Settings
        {
            public RectTransform mainCanvasTransform;
            public GameObject headerCanvasPrefab;
            public GameObject centerCanvasPrefab;
            public RectTransform coinParent;
            public GameObject uiCoinPrefab;
        }

        [SerializeField] private Settings settings;

        public override void InstallBindings()
        {
            //todo: bind ui classes here
            Container.Bind<UIHeaderCanvas>()
                .FromComponentInNewPrefab(settings.headerCanvasPrefab)
                .UnderTransform(settings.mainCanvasTransform)
                .AsSingle();

            Container.BindInterfacesTo<UIPlayerStatusHandler>().AsSingle();

            Container.BindInterfacesAndSelfTo<UICoinManager>().AsSingle().WithArguments(settings.mainCanvasTransform);
            Container.BindInterfacesTo<UICurrencyHandler>().AsSingle();

            Container.BindFactory<CoinSpawnParams, UICoin, UICoin.Factory>()
                .FromPoolableMemoryPool<CoinSpawnParams, UICoin, UICoinPool>(poolBinder => poolBinder
                    .WithInitialSize(20)
                    .FromComponentInNewPrefab(settings.uiCoinPrefab)
                    .UnderTransform(settings.coinParent));

            Container.Bind<UICenterCanvas>()
                .FromComponentInNewPrefab(settings.centerCanvasPrefab)
                .UnderTransform(settings.mainCanvasTransform)
                .AsSingle();

            Container.BindInterfacesTo<UIGameStatusHandler>().AsSingle();

            Container.BindInterfacesAndSelfTo<UIMainCanvas>().AsSingle().WithArguments(settings.mainCanvasTransform);

            //this is just bound to trigger gameSceneSetup event
            Container.BindInterfacesTo<LateInitializer>().AsSingle();
        }
        
        public class UICoinPool : MonoPoolableMemoryPool<CoinSpawnParams, IMemoryPool, UICoin>
        {
        }
    }   
}

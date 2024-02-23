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
            public RectTransform MainCanvasTransform;
            public GameObject HeaderCanvasPrefab;
            public GameObject UICoinPrefab;
        }

        [SerializeField] private Settings settings;

        public override void InstallBindings()
        {
            //todo: bind ui classes here
            Container.Bind<UIHeaderCanvas>()
                .FromComponentInNewPrefab(settings.HeaderCanvasPrefab)
                .UnderTransform(settings.MainCanvasTransform).AsSingle();

            Container.BindInterfacesTo<UIPlayerStatusHandler>().AsSingle();

            Container.BindInterfacesAndSelfTo<UICoinManager>().AsSingle().WithArguments(settings.MainCanvasTransform);
            Container.BindInterfacesTo<UICurrencyHandler>().AsSingle();

            Container.BindFactory<CoinSpawnParams, UICoin, UICoin.Factory>()
                .FromPoolableMemoryPool<CoinSpawnParams, UICoin, UICoinPool>(poolBinder => poolBinder
                    .WithInitialSize(20)
                    .FromComponentInNewPrefab(settings.UICoinPrefab)
                    .UnderTransform(settings.MainCanvasTransform));
        }
        
        public class UICoinPool : MonoPoolableMemoryPool<CoinSpawnParams, IMemoryPool, UICoin>
        {
        }
    }   
}

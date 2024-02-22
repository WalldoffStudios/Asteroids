using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class GameInstaller : MonoInstaller
    {
        [Serializable]
        public class Settings
        {
            public GameObject AsteroidPrefab;
        }
        
        [Inject]
        private Settings _settings;

        public override void InstallBindings()
        {
            //Container.Bind<ScreenBorders>().AsSingle().WithArguments(Camera.main);
            Container.BindInterfacesAndSelfTo<ScreenBorders>().AsSingle().WithArguments(Camera.main);

            Container.BindFactory<AsteroidSpawnParams, Asteroid, Asteroid.Factory>()
                .FromPoolableMemoryPool<AsteroidSpawnParams, Asteroid, AsteroidPool>(poolBinder => poolBinder
                    .WithInitialSize(20)
                    .FromComponentInNewPrefab(_settings.AsteroidPrefab)
                    .UnderTransformGroup("Asteroids"));

            Container.BindInterfacesAndSelfTo<AsteroidManager>().AsSingle();
            
            GameSignalsInstaller.Install(Container);
        }
        
        public class AsteroidPool : MonoPoolableMemoryPool<AsteroidSpawnParams, IMemoryPool, Asteroid>
        {
        }
    }   
}

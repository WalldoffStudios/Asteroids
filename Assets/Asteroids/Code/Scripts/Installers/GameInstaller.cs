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
            Container.Bind<ScreenBorders>().AsSingle().WithArguments(Camera.main);
            GameSignalsInstaller.Install(Container);
        }
    }   
}

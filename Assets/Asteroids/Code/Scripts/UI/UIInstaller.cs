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
            public Transform MainCanvasTransform;
            public GameObject HeaderCanvasPrefab;
            //public UIHeaderCanvas HeaderCanvasPrefab;
            public GameObject BottomCanvasPrefab;
        }

        [SerializeField] private Settings settings;

        public override void InstallBindings()
        {
            //todo: bind ui classes here
            Container.Bind<UIHeaderCanvas>()
                .FromComponentInNewPrefab(settings.HeaderCanvasPrefab)
                .UnderTransform(settings.MainCanvasTransform).AsSingle();

            Container.BindInterfacesTo<UIPlayerStatusHandler>().AsSingle();
        }
    }   
}

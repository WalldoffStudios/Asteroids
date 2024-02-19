using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerInstaller : MonoInstaller
    {
        //try using a struct instead
        [Serializable]
        public class Settings
        {
            public Rigidbody2D rigidBody;
            public SpriteRenderer renderer;
        }

        [SerializeField] private Settings settings = null;

        public override void InstallBindings()
        {
            Container.Bind<Player>().AsSingle().WithArguments(settings.rigidBody, settings.renderer);
            Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHealthHandler>().AsSingle();
            
        }
    }   
}

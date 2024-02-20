using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerInstaller : MonoInstaller
    {
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
            Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerRotationHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHealthHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerBordersHandler>().AsSingle();
            
            Container.Bind<PlayerInputState>().AsSingle();
        }
    }   
}

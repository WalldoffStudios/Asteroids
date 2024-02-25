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
            public Collider2D collider;
            public Rigidbody2D rigidBody;
            public SpriteRenderer renderer;
            public Transform shootPoint;
            public LayerMask collisionLayers;
        }

        [SerializeField] private Settings settings = null;

        public override void InstallBindings()
        {
            Container.Bind<Player>().AsSingle().WithArguments(settings);
            Container.BindInterfacesAndSelfTo<PlayerFacade>().FromComponentInChildren().AsSingle();
            Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerRotationHandler>().AsSingle().WithArguments(Camera.main);
            Container.BindInterfacesAndSelfTo<PlayerHealthHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerBordersHandler>().AsSingle();
            Container.Bind<IWeapon>().To<LazerWeapon>().AsSingle();
            Container.BindInterfacesTo<PlayerWeaponHandler>().AsSingle();
            Container.Bind<PlayerInputState>().AsSingle();
        }
    }   
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids
{
    public class Player
    {
        private readonly Collider2D _collider;
        private readonly Rigidbody2D _rigidBody;
        public SpriteRenderer Renderer { get; }
        public Transform ShootPoint { get; }
        public LayerMask CollisionLayers { get; }
        
        public Player(PlayerInstaller.Settings settings, PlayerTextureReference playerData)
        {
            _collider = settings.collider;
            _rigidBody = settings.rigidBody;
            Renderer = settings.renderer;
            ShootPoint = settings.shootPoint;
            CollisionLayers = settings.collisionLayers;
            Renderer.sprite = playerData.PlayerTexture;
        }
        public bool IsDead { get; private set; }
        
        public float Rotation
        {
            get => _rigidBody.rotation;
            set => _rigidBody.rotation = value;
        }

        public Vector2 Position
        {
            get => _rigidBody.position;
            set => _rigidBody.position = value;
        }

        public Vector2 Velocity => _rigidBody.velocity;

        public void Bounce(Vector2 direction) => _rigidBody.velocity = direction;

        public void AddForce(Vector2 force)
        {
            _rigidBody.AddForce(force, ForceMode2D.Force);
            _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, 10.0f);
        }

        public void Died()
        {
            IsDead = true;
            _rigidBody.velocity = Vector2.zero;
            _collider.enabled = false;
            Renderer.enabled = false;
        }
    }   
}

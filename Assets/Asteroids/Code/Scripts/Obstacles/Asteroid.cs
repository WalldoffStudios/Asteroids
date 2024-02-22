using UnityEngine;
using Zenject;

namespace Asteroids
{
    public struct AsteroidSpawnParams
    {
        public readonly int Id;
        public readonly float Speed;
        public readonly float Size;
        public readonly int CollisionDamage; 
        public readonly LayerMask CollisionLayers;

        public AsteroidSpawnParams(
            int id,
            float speed,
            float size,
            int collisionDamage,
            LayerMask collisionLayers)
        {
            Id = id;
            Speed = speed;
            Size = size;
            CollisionDamage = collisionDamage;
            CollisionLayers = collisionLayers;
        }
    }
    
    public class Asteroid : MonoBehaviour, IPoolable<AsteroidSpawnParams, IMemoryPool>, IDamageAble
    {
        [SerializeField] private Rigidbody2D rigidBody = null;

        [Inject]
        private SignalBus _signalBus;
        
        public Vector2 Velocity => rigidBody.velocity;
        public float Size { get; private set; }
        private bool _hasCollided;

        private int _id;
        private float _speed;
        private int _collisionDamage;
        private LayerMask _playerLayer;
        private IMemoryPool _pool;
        
        public void OnSpawned(AsteroidSpawnParams spawnParams, IMemoryPool pool)
        {
            _id = spawnParams.Id;
            _speed = spawnParams.Speed;
            _collisionDamage = spawnParams.CollisionDamage;
            Size = spawnParams.Size;
            _playerLayer = spawnParams.CollisionLayers;
            _pool = pool;
            transform.localScale = Vector2.one * Size;
            _hasCollided = false;
        }
        
        public void OnDespawned()
        {
            rigidBody.velocity = Vector2.zero;
            _pool = null;
        }
        
        public void UpdateDirection(Vector2 direction)
        {
            rigidBody.velocity = direction * _speed;
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, _speed);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_hasCollided == true) return;
            if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                IDamageAble damageAble = other.gameObject.GetComponent<IDamageAble>();
                if (damageAble != null)
                {
                    Debug.Log("Collided with player and tried to damage it");
                    damageAble.TakeDamage(_collisionDamage);
                }
                else
                {
                    Debug.LogError("Collided with object on collision layer but it didnt have IDamageable interface");
                }
            }
        }
        
        public void TakeDamage(int amount)
        {
            Despawn();
        }

        private void Despawn()
        {
            _hasCollided = true;
            
            Transform asteroidTransform = transform;
            _signalBus.Fire(new ObjectDestroyed(_id, asteroidTransform.position, asteroidTransform.localScale.x));
            
            _pool.Despawn(this);
        }
        
        public class Factory : PlaceholderFactory<AsteroidSpawnParams, Asteroid>
        {
        }
    }   
}

using UnityEngine;
using Zenject;

namespace Asteroids
{
    public struct AsteroidSpawnParams
    {
        public int Id;
        public readonly float Speed;
        public readonly float Size;
        public LayerMask PlayerLayerMask;
        public LayerMask EnemyLayerMask;
        public LayerMask ProjectileLayerMask;

        public AsteroidSpawnParams(
            int id,
            float speed,
            float size,
            LayerMask player,
            LayerMask enemy,
            LayerMask projectile)
        {
            Id = id;
            Speed = speed;
            Size = size;
            PlayerLayerMask = player;
            EnemyLayerMask = enemy;
            ProjectileLayerMask = projectile;
        }
    }
    
    public class Asteroid : MonoBehaviour, IPoolable<AsteroidSpawnParams, IMemoryPool>
    {
        [SerializeField] private Rigidbody2D rigidBody = null;

        [Inject]
        private SignalBus _signalBus;
        
        public Vector2 Velocity => rigidBody.velocity;

        private int _id;
        private float _speed;
        private float _size;
        private LayerMask _playerLayer;
        private LayerMask _enemyLayer;
        private LayerMask _projectileLayer;
        private IMemoryPool _pool;
        
        public void OnSpawned(AsteroidSpawnParams spawnParams, IMemoryPool pool)
        {
            _id = spawnParams.Id;
            _speed = spawnParams.Speed;
            _size = spawnParams.Size;
            _playerLayer = spawnParams.PlayerLayerMask;
            _enemyLayer = spawnParams.EnemyLayerMask;
            _projectileLayer = spawnParams.ProjectileLayerMask;
            _pool = pool;
            transform.localScale = Vector2.one * _size;
        }
        
        public void OnDespawned()
        {
            rigidBody.velocity = Vector2.zero;
            _pool = null;
        }
        
        public void UpdateDirection(Vector2 direction)
        {
            Debug.Log($"Direction pass in was {direction}");
            rigidBody.velocity = direction * _speed;
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, _speed);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                Debug.Log("Collided with a player");
                Despawn();
            }
            else if ((_enemyLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                Debug.Log("Collided with a enemy");
                Despawn();
            }
            else if ((_projectileLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                Debug.Log("Collided with a projectile");
                Despawn();
            }
        }

        private void Despawn()
        {
            //todo: play explosion vfx
            var transform1 = transform;
            // if (transform1.localScale.x > 1.0f)
            // {
            //     // _signalBus.Fire(new ObstacleDestroyed(transform1.position, transform1.localScale.x));   
            // }
            _signalBus.Fire(new ObstacleDestroyed(_id, transform1.position, transform1.localScale.x));
            _pool.Despawn(this);
        }
        
        public class Factory : PlaceholderFactory<AsteroidSpawnParams, Asteroid>
        {
        }
    }   
}

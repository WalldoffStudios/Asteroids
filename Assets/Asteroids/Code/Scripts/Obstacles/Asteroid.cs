using UnityEngine;
using Zenject;

namespace Asteroids
{
    public struct AsteroidSpawnParams
    {
        public readonly float Speed;
        public readonly float Size;
        public LayerMask PlayerLayerMask;
        public LayerMask EnemyLayerMask;
        public LayerMask ProjectileLayerMask;

        public AsteroidSpawnParams(
            float speed,
            float size,
            LayerMask player,
            LayerMask enemy,
            LayerMask projectile)
        {
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
        
        private float _speed;
        private float _size;
        private LayerMask _playerLayer;
        private LayerMask _enemyLayer;
        private LayerMask _projectileLayer;
        private IMemoryPool _pool;
        
        public void OnSpawned(AsteroidSpawnParams spawnParams, IMemoryPool pool)
        {
            _speed = spawnParams.Speed;
            _size = spawnParams.Size;
            _playerLayer = spawnParams.PlayerLayerMask;
            _enemyLayer = spawnParams.EnemyLayerMask;
            _projectileLayer = spawnParams.ProjectileLayerMask;
            _pool = pool;
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }
        
        //public Vector2 Velocity { get; set; }
        public Vector2 Velocity => rigidBody.velocity;
        public void UpdateDirection(Vector2 direction)
        {
            rigidBody.velocity = direction;
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
            _pool.Despawn(this);
        }
        
        public class Factory : PlaceholderFactory<AsteroidSpawnParams, Asteroid>
        {
        }
    }   
}

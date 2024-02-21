using UnityEngine;
using Zenject;

namespace Asteroids
{
    public struct AsteroidSpawnParams
    {
        public float Speed;
        public float Size;
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
    
    public class Asteroid : MonoBehaviour, IPoolable<float, float, IMemoryPool>
    {
        [SerializeField] private Rigidbody2D rigidBody = null;
        
        private float _speed;
        private float _size;
        private IMemoryPool _pool;
        
        public void OnSpawned(float speed, float size, IMemoryPool pool)
        {
            _speed = speed;
            _size = size;
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
            _pool.Despawn(this);
        }
        
        public class Factory : PlaceholderFactory<float, float, Asteroid>
        {
        }
    }   
}

using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public struct BulletSpawnParams
    {
        public float Damage;
        public float Speed;
        public LayerMask CollisionLayers;

        public BulletSpawnParams(float damage, float speed, LayerMask collisionLayers)
        {
            Damage = damage;
            Speed = speed;
            CollisionLayers = collisionLayers;
        }
    }
    public class LazerBullet : MonoBehaviour, IPoolable<BulletSpawnParams, IMemoryPool>
    {
        [SerializeField] private Rigidbody2D rigidBody = null;

        private float _damage;
        private float _speed;
        private LayerMask _collisionLayers;
        private IMemoryPool _pool;
        
        public void OnSpawned(BulletSpawnParams spawnParams, IMemoryPool pool)
        {
            _damage = spawnParams.Damage;
            _speed = spawnParams.Speed;
            _collisionLayers = spawnParams.CollisionLayers;
            _pool = pool;
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }

        public void SetDirection(Vector2 direction)
        {
            Vector2 velocity = direction * _speed;
            rigidBody.velocity = velocity;
            
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 70.0f;
            rigidBody.rotation = angle;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((_collisionLayers.value & (1 << other.gameObject.layer)) != 0)
            {
                
                if(_pool != null) _pool.Despawn(this);   
            }
        }

        public class Factory : PlaceholderFactory<BulletSpawnParams, LazerBullet>
        {
        }
    }   
}

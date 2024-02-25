using UnityEngine;
using Zenject;

namespace Asteroids
{
    public struct BulletSpawnParams
    {
        public int Damage { get; }
        public float Speed{ get; }
        public LayerMask CollisionLayers{ get; }

        public BulletSpawnParams(int damage, float speed, LayerMask collisionLayers)
        {
            Damage = damage;
            Speed = speed;
            CollisionLayers = collisionLayers;
        }
    }
    public class LazerBullet : MonoBehaviour, IPoolable<BulletSpawnParams, IMemoryPool>
    {
        [SerializeField] private Rigidbody2D rigidBody = null;

        private int _damage;
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
            rigidBody.AddForce(direction * _speed, ForceMode2D.Impulse);

            Vector2 velocity = rigidBody.velocity;
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90.0f;
            rigidBody.rotation = angle;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((_collisionLayers.value & (1 << other.gameObject.layer)) != 0)
            {
                IDamageAble damageAble = other.gameObject.GetComponent<IDamageAble>();
                if (damageAble != null)
                {
                    damageAble.TakeDamage(_damage);
                }
                if(_pool != null) _pool.Despawn(this);   
            }
        }

        public class Factory : PlaceholderFactory<BulletSpawnParams, LazerBullet>
        {
        }
    }   
}
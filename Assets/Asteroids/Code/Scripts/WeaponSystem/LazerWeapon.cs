using UnityEngine;

namespace Asteroids
{
    public class LazerWeapon : IWeapon
    {
        private readonly LazerBullet.Factory _bulletFactory;
        private readonly Player _player;
        
        public LazerWeapon(LazerBullet.Factory bulletFactory, Player player)
        {
            _bulletFactory = bulletFactory;
            _player = player;
        }
        
        public void Fire()
        {
            BulletSpawnParams spawnParams = new BulletSpawnParams(2, 20.0f, _player.CollisionLayers);
            LazerBullet bullet = _bulletFactory.Create(spawnParams);

            Vector2 position = _player.ShootPoint.position;
            Vector2 direction = ((Vector2)position - _player.Position).normalized;
            bullet.transform.position = position;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            
            bullet.SetDirection(direction);
        }
    }   
}

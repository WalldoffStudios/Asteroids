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
            BulletSpawnParams spawnParams = new BulletSpawnParams(2.0f, 10.0f, _player.CollisionLayers);
            LazerBullet bullet = _bulletFactory.Create(spawnParams);

            Vector2 normalizedVelocity = _player.Velocity.normalized;
            bullet.transform.position = _player.Position + normalizedVelocity * 0.6f;
            
            //player is rotated towards its velocity so we can use it to spawn bullet at correct spot
            bullet.SetDirection(normalizedVelocity);
        }
    }   
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class AsteroidManager : IInitializable, ITickable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            public float minSpeed;
            public float maxSpeed;
            public float minSize;
            public float maxSize;
            public LayerMask playerMask;
            public LayerMask enemyMask;
            public LayerMask projectileMask;
            public bool bounceOnBorders;
            public float borderCheckDelay;
        }
        
        private float _ticksSinceLastPositionUpdate;
        
        private readonly Asteroid.Factory _asteroidFactory;
        private readonly ScreenBorders _screenBorders;
        private readonly Settings _settings;
        private readonly List<Asteroid> _activeAsteroids = new List<Asteroid>();
        private readonly float _spawnThreshold = 10; // minimum number of asteroids

        public AsteroidManager(Asteroid.Factory asteroidFactory, ScreenBorders borders, Settings settings)
        {
            _asteroidFactory = asteroidFactory;
            _screenBorders = borders;
            _settings = settings;
        }
        
        public void Initialize()
        {
            EnsureAsteroids();
        }

        public void Tick()
        {
            _ticksSinceLastPositionUpdate += Time.deltaTime;
            if (_ticksSinceLastPositionUpdate > _settings.borderCheckDelay)
            {
                BorderCheck();
                _ticksSinceLastPositionUpdate = 0.0f;
            }
        }

        public void Dispose()
        {
            //todo: eventual cleanup
        }

        private void BorderCheck()
        {
            foreach (var asteroid in _activeAsteroids)
            {
                if (_screenBorders.IsNearEdge(asteroid.transform.position))
                {
                    if (_settings.bounceOnBorders == true)
                    {
                        BounceAsteroid(asteroid);
                    }
                    else
                    {
                        TeleportAsteroid(asteroid);            
                    }
                }
            }
        }

        private void BounceAsteroid(Asteroid asteroid)
        {
            Vector2 bounceDirection =
                _screenBorders.GetBounceDirection(asteroid.transform.position, asteroid.Velocity);
                    
            asteroid.UpdateDirection(bounceDirection);
        }

        private void TeleportAsteroid(Asteroid asteroid)
        {
            Vector2 teleportPosition = _screenBorders.GetTeleportPosition(asteroid.transform.position);
            asteroid.transform.position = teleportPosition;
            Debug.Log($"tried to teleport asteroid to location: {teleportPosition}");
        }
        
        private void EnsureAsteroids()
        {
            while (_activeAsteroids.Count < _spawnThreshold)
            {
                
                AsteroidSpawnParams spawnParams = new AsteroidSpawnParams(
                    Random.Range(_settings.minSpeed, _settings.maxSpeed),
                    Random.Range(_settings.minSize, _settings.maxSize),
                    _settings.playerMask,
                    _settings.enemyMask,
                    _settings.projectileMask
                );
                
                Asteroid asteroid = _asteroidFactory.Create(spawnParams);
                asteroid.transform.position = _screenBorders.GetRandomPositionWithinScreen();
                _activeAsteroids.Add(asteroid);
            }
            UpdateAsteroidDirections();
        }

        private void UpdateAsteroidDirections()
        {
            foreach (var asteroid in _activeAsteroids)
            {
                Vector2 direction = Random.insideUnitCircle.normalized;
                asteroid.UpdateDirection(direction);
            }
        }

        // Call this from somewhere when an asteroid is destroyed or leaves the screen
        public void RemoveAsteroid(Asteroid asteroid)
        {
            _activeAsteroids.Remove(asteroid);
        }
    }   
}

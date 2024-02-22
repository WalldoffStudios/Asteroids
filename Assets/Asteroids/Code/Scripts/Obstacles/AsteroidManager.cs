using System;
using System.Collections.Generic;
using UnityEngine;
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
            public int maxAsteroidCount;
            public int minAsteroidCount;
            public float asteroidSpawnDelay;
        }
        
        private float _ticksSinceLastPositionUpdate;
        private float _ticksSinceLastSpawn;
        private int _spawnedAsteroids;
        
        private readonly Asteroid.Factory _asteroidFactory;
        private readonly ScreenBorders _screenBorders;
        private readonly Settings _settings;
        private readonly SignalBus _signalBus;
        private readonly Dictionary<int, Asteroid> _activeAsteroids = new Dictionary<int, Asteroid>();
        //private readonly List<Asteroid> _activeAsteroids = new List<Asteroid>();

        public AsteroidManager(
            Asteroid.Factory asteroidFactory,
            ScreenBorders borders,
            Settings settings,
            SignalBus signalBus
            )
        {
            _asteroidFactory = asteroidFactory;
            _screenBorders = borders;
            _settings = settings;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            SpawnNewAsteroids();
            _signalBus.Subscribe<ObstacleDestroyed>(OnAsteroidDestroyed);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<ObstacleDestroyed>(OnAsteroidDestroyed);
        }
        
        private void OnAsteroidDestroyed(ObstacleDestroyed signal)
        {
            if (signal.Size < 1.0f)
            {
                if (_activeAsteroids.ContainsKey(signal.ObjectId) == true)
                {
                    _activeAsteroids.Remove(signal.ObjectId);
                }
                else
                {
                    Debug.LogError("Tried to remove asteroid that didnt even exist in dictionary");
                }
                return;
            }
            
            float speed = Random.Range(_settings.minSpeed, _settings.maxSpeed);
            float size = signal.Size / 2.0f;
            int numberOfAsteroids = 5;
            float angleIncrement = 360.0f / numberOfAsteroids;
            for (int i = 0; i < numberOfAsteroids; i++)
            {
                float angleRadians = Mathf.Deg2Rad * (angleIncrement * i);
                Vector2 spawnDirection = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
                Vector3 spawnPosition = signal.Position + (spawnDirection * 0.25f) ;
                
                Asteroid asteroid = SpawnAsteroid(speed, size);
                asteroid.transform.position = spawnPosition;
                asteroid.UpdateDirection(spawnDirection.normalized);
            }
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            
            TickPositionUpdates(deltaTime);
            TickSpawnTimer(deltaTime);
        }

        private void TickPositionUpdates(float deltaTime)
        {
            _ticksSinceLastPositionUpdate += deltaTime;
            if (_ticksSinceLastPositionUpdate > _settings.borderCheckDelay)
            {
                BorderCheck();
                _ticksSinceLastPositionUpdate = 0.0f;
            }
        }

        private void TickSpawnTimer(float deltaTime)
        {
            _ticksSinceLastSpawn += deltaTime;
            if (_ticksSinceLastSpawn > _settings.asteroidSpawnDelay)
            {
                SpawnNewAsteroids();
                _ticksSinceLastSpawn = 0.0f;
            }
        }

        private void BorderCheck()
        {
            foreach (var keyValuePair in _activeAsteroids)
            {
                Asteroid asteroid = keyValuePair.Value;
                if (_screenBorders.IsInsideScreenBounds(asteroid.transform.position) == false) continue;
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
        }
        
        private void SpawnNewAsteroids()
        {
            int activeAsteroids = _activeAsteroids.Count;
            if (activeAsteroids < _settings.minAsteroidCount)
            {
                for (int i = activeAsteroids; i <= _settings.minAsteroidCount; i++)
                {

                    Asteroid asteroid = SpawnAsteroid(
                        Random.Range(_settings.minSpeed, _settings.maxSpeed), 
                        Random.Range(_settings.minSize, _settings.maxSize));
                    
                    Vector2 asteroidPos = _screenBorders.GetRandomPositionOutsideScreen();
                    asteroid.transform.position = asteroidPos;
                    Vector2 directionToScreen = (_screenBorders.GetRandomPositionWithinScreen() - asteroidPos).normalized;
                    asteroid.UpdateDirection(directionToScreen);
                    
                    // _activeAsteroids.Add(_spawnedAsteroids, asteroid);    
                }
            
                // UpdateAsteroidDirections();   
            }
        }

        private Asteroid SpawnAsteroid(float speed, float size)
        {
            AsteroidSpawnParams spawnParams = new AsteroidSpawnParams(
                _spawnedAsteroids,
                speed,
                size,
                _settings.playerMask,
                _settings.enemyMask,
                _settings.projectileMask
            );
                
            Asteroid asteroid = _asteroidFactory.Create(spawnParams);
            _activeAsteroids.Add(_spawnedAsteroids, asteroid);
            _spawnedAsteroids++;
            return asteroid;
        }
    }   
}

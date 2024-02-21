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
            //public int 
        }
        
        private readonly Asteroid.Factory _asteroidFactory;
        private readonly ScreenBorders _screenBorders;
        private readonly List<Asteroid> _activeAsteroids = new List<Asteroid>();
        private readonly float _spawnThreshold = 10; // minimum number of asteroids

        public AsteroidManager(Asteroid.Factory asteroidFactory, ScreenBorders borders)
        {
            _asteroidFactory = asteroidFactory;
            _screenBorders = borders;
        }

        private float ticksSinceLastPositionUpdate;
        
        public void Initialize()
        {
            EnsureAsteroids();
        }

        public void Tick()
        {
            ticksSinceLastPositionUpdate += Time.deltaTime;
            if (ticksSinceLastPositionUpdate > 0.1f)
            {
                MaintainAsteroidsInScreenSpace();
                ticksSinceLastPositionUpdate = 0.0f;
            }
        }

        public void Dispose()
        {
            
        }

        private void MaintainAsteroidsInScreenSpace()
        {
            for (int i = 0; i < _activeAsteroids.Count; i++)
            {
                Asteroid asteroid = _activeAsteroids[i];
                if (_screenBorders.IsNearEdge(asteroid.transform.position))
                {
                    Vector2 bounceDirection =
                        _screenBorders.GetBounceDirection(asteroid.transform.position, asteroid.Velocity);
                    
                    asteroid.UpdateDirection(bounceDirection);
                }   
            }
        }
        
        private void EnsureAsteroids()
        {
            while (_activeAsteroids.Count < _spawnThreshold)
            {
                var asteroid = _asteroidFactory.Create(Random.Range(1f, 3f), Random.Range(0.5f, 1.5f));
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

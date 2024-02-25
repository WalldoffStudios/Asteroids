using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class ScreenBorders : IInitializable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            public float minZoomLevel;
            public float maxZoomLevel;
            public float edgeCheckDistance;
            public float teleportEdgeCheckDistance;
            public float teleportDistance;
            public float spawnBorderOffset;
        }
        
        private readonly Camera _camera;
        private readonly SignalBus _signalBus;
        private readonly Settings _settings;
        public ScreenBorders(Camera camera, SignalBus signalBus, Settings settings)
        {
            _camera = camera;
            _signalBus = signalBus;
            _settings = settings;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<CameraZoomSignal>(UpdateCameraValues);
            _extentHeight = _camera.orthographicSize;
            _extentWidth = _camera.aspect * _camera.orthographicSize;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<CameraZoomSignal>(UpdateCameraValues);
        }

        private void UpdateCameraValues(CameraZoomSignal signal)
        {
            _camera.orthographicSize = Mathf.Clamp(
                _camera.orthographicSize + signal.ZoomLevel,
                _settings.minZoomLevel,
                _settings.maxZoomLevel);

            _extentHeight = _camera.orthographicSize;
            _extentWidth = _camera.aspect * _camera.orthographicSize;
        }
        private float Bottom => -_extentHeight;
        private float Top => _extentHeight;
        private float Left => -_extentWidth;

        private float Right => _extentWidth;
        private float _extentHeight;
        private float _extentWidth;

        public bool IsInsideScreenBounds(Vector2 position)
        {
            float x = position.x;
            float y = position.y;
            return x > Left && x < Right && y < Top && y > Bottom;
        }
        
        public bool NearHorizontalEdge(float xPosition) => Mathf.Abs(xPosition) > Right - _settings.edgeCheckDistance;
        public bool NearVerticalEdge(float yPosition) => Mathf.Abs(yPosition) > Top - _settings.edgeCheckDistance;
        
        public bool IsNearInsideEdge(Vector2 position)
        {
            return NearHorizontalEdge(position.x) == true || NearVerticalEdge(position.y) == true;
        }

        public bool IsWithinTeleportEdge(Vector2 position, float size)
        {
            float x = position.x;
            float y = position.y;
            float edgeDistance = _settings.teleportEdgeCheckDistance * size;
            
            return x > Left - edgeDistance && x < Right + edgeDistance && y < Top + edgeDistance && y > Bottom - edgeDistance;
        }

        public Vector2 GetRandomPositionWithinScreen()
        {
            float randomX = Random.Range(0.1f, 0.9f);
            float randomY = Random.Range(0.1f, 0.9f);
            Vector2 screenPos = _camera.ViewportToWorldPoint(new Vector3(randomX, randomY, 0.0f));
            return screenPos;
        }
        
        public Vector2 GetRandomPositionOutsideScreen()
        {
            float randomX = Random.Range(-_settings.spawnBorderOffset, _settings.spawnBorderOffset);
            float randomY = Random.Range(-_settings.spawnBorderOffset, _settings.spawnBorderOffset);
            randomX += randomX > 0.0f ? 1.0f : -1.0f;
            randomY += randomY > 0.0f ? 1.0f : -1.0f;
            Vector2 screenPos = _camera.ViewportToWorldPoint(new Vector3(randomX, randomY, 0.0f));
            return screenPos;
        }

        public Vector2 GetBounceDirection(Vector2 position, Vector2 currentDirection)
        {
            Vector2 newDirection = currentDirection;
            
            if (NearHorizontalEdge(position.x) == true)
            {
                newDirection.x = position.x > 0.0f ? -1.0f : 1.0f;
            }
            if (NearVerticalEdge(position.y) == true)
            {
                newDirection.y = position.y > 0.0f ? -1.0f : 1.0f;
            }
            
            return newDirection.normalized;
        }
        
        public Vector2 GetTeleportPosition(Vector2 currentPosition, Vector2 direction, float sizeOfObject)
        {
            Vector2 teleportPosition = currentPosition;
            float edgeDistance = _settings.teleportDistance * sizeOfObject;
            
            // Check if the asteroid is moving towards the edges, and teleport only if it's moving outwards
            if (NearHorizontalEdge(currentPosition.x) && (currentPosition.x > 0 && direction.x > 0 || currentPosition.x < 0 && direction.x < 0))
            {
                // Teleport to the opposite horizontal edge
                teleportPosition.x = currentPosition.x > 0 ? Left - edgeDistance : Right + edgeDistance;
            }
            if (NearVerticalEdge(currentPosition.y) && (currentPosition.y > 0 && direction.y > 0 || currentPosition.y < 0 && direction.y < 0))
            {
                // Teleport to the opposite vertical edge
                teleportPosition.y = currentPosition.y > 0 ? Bottom - edgeDistance : Top + edgeDistance;
            }

            return teleportPosition;
        }
    }   
}

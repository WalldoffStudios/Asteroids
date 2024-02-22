using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class ScreenBorders : IInitializable, IDisposable
    {
        private readonly Camera _camera;
        private readonly SignalBus _signalBus;
        public ScreenBorders(Camera camera, SignalBus signalBus)
        {
            _camera = camera;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<CameraZoomSignal>(UpdateCameraValues);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<CameraZoomSignal>(UpdateCameraValues);
        }

        private void UpdateCameraValues(CameraZoomSignal signal)
        {
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + signal.ZoomLevel, 5.0f, 100.0f);
        }
        
        public float Bottom => -ExtentHeight;

        public float Top => ExtentHeight;

        public float Left => -ExtentWidth;

        public float Right => ExtentWidth;

        public float ExtentHeight => _camera.orthographicSize;

        public float Height => ExtentHeight * 2.0f;

        public float ExtentWidth => _camera.aspect * _camera.orthographicSize;

        public float Width => ExtentWidth * 2.0f;


        public bool IsInsideScreenBounds(Vector2 position)
        {
            float x = position.x;
            float y = position.y;
            return x > Left && x < Right && y < Top && y > Bottom;
        }
        public bool IsNearEdge(Vector2 position)
        {
            bool nearHorizontalEdge = Mathf.Abs(position.x) > Right - 0.3f;
            bool nearVerticalEdge = Mathf.Abs(position.y) > Top - 0.3f;

            return nearHorizontalEdge || nearVerticalEdge;
        }

        public Vector2 GetRandomPositionWithinScreen()
        {
            float randomX = Random.Range(0.1f, 0.9f);
            float randomy = Random.Range(0.1f, 0.9f);
            Vector2 screenPos = _camera.ViewportToWorldPoint(new Vector3(randomX, randomy, 0.0f));
            return screenPos;
        }
        
        public Vector2 GetRandomPositionOutsideScreen()
        {
            float randomX = Random.Range(-0.3f, 0.3f);
            float randomY = Random.Range(-0.3f, 0.3f);
            randomX += randomX > 0.0f ? 1.0f : -1.0f;
            randomY += randomY > 0.0f ? 1.0f : -1.0f;
            Vector2 screenPos = _camera.ViewportToWorldPoint(new Vector3(randomX, randomY, 0.0f));
            return screenPos;
        }

        public Vector2 GetBounceDirection(Vector2 position, Vector2 currentDirection)
        {
            Vector2 newDirection = currentDirection;
            
            bool nearHorizontalEdge = Mathf.Abs(position.x) > Right - 0.3f;
            if (nearHorizontalEdge == true)
            {
                newDirection.x = position.x > 0.0f ? -1.0f : 1.0f;
            }
            bool nearVerticalEdge = Mathf.Abs(position.y) > Top - 0.3f;
            if (nearVerticalEdge == true)
            {
                newDirection.y = position.y > 0.0f ? -1.0f : 1.0f;
            }
            
            return newDirection.normalized;
        }
        
        //todo: fix this to use camera extent values instead
        public Vector3 GetTeleportPosition(Vector3 currentPosition)
        {
            Vector3 viewportPosition = _camera.WorldToViewportPoint(currentPosition);

            // Invert the viewport position if it's outside the 0-1 range
            viewportPosition.x = viewportPosition.x < 0 || viewportPosition.x > 1 ? 1 - viewportPosition.x : viewportPosition.x;
            viewportPosition.y = viewportPosition.y < 0 || viewportPosition.y > 1 ? 1 - viewportPosition.y : viewportPosition.y;

            // Ensure the viewport position is within bounds to avoid teleporting when in the center
            viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.01f, 0.99f);
            viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0.01f, 0.99f);

            // Convert the adjusted viewport position back to world space
            Vector3 targetWorldPosition = _camera.ViewportToWorldPoint(viewportPosition);
            targetWorldPosition.z = currentPosition.z; // Preserve the original Z coordinate

            return targetWorldPosition;
        }
    }   
}

using UnityEngine;

namespace Asteroids
{
    public class ScreenBorders
    {
        private readonly Camera _camera;

        public ScreenBorders(Camera camera) => _camera = camera;
        public float ExtentHeight => _camera.orthographicSize;
        public float ExtentWidth => _camera.aspect * _camera.orthographicSize;

        public float Height => ExtentHeight * 2.0f;
        public float ScreenWidth => ExtentWidth * 2.0f;
        
        public float Bottom => -ExtentHeight;

        public float Top => ExtentHeight;

        public float Left => -ExtentWidth;

        public float Right => ExtentWidth;

        public bool IsNearEdge(Vector3 position)
        {
            Vector3 screenPosition = _camera.WorldToViewportPoint(position);
            return 
                screenPosition.x < 0.1f || 
                screenPosition.x > 0.9f || 
                screenPosition.y < 0.1f || 
                screenPosition.y > 0.9f;
        }

        public Vector3 GetBounceDirection(Vector3 position, Vector3 currentDirection)
        {
            Vector3 screenPosition = _camera.WorldToViewportPoint(position);
            Vector3 newDirection = currentDirection;
            
            if (screenPosition.x is < 0.01f or > 0.99f)
            {
                newDirection.x = -newDirection.x;
            }
            if (screenPosition.y is < 0.01f or > 0.99f)
            {
                newDirection.y = -newDirection.y;
            }

            return newDirection;
        }
    }   
}

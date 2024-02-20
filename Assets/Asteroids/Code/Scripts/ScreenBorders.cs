using UnityEngine;

namespace Asteroids
{
    public class ScreenBorders
    {
        private readonly Camera _camera;
        public ScreenBorders(Camera camera) => _camera = camera;

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

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
                screenPosition.x < 0.01f || 
                screenPosition.x > 0.99f || 
                screenPosition.y < 0.01f || 
                screenPosition.y > 0.99f;
        }

        public Vector3 GetRandomPositionWithinScreen()
        {
            float randomX = Random.Range(0.1f, 0.9f);
            float randomy = Random.Range(0.1f, 0.9f);
            Vector2 screenPos = _camera.ViewportToWorldPoint(new Vector3(randomX, randomy, 0.0f));
            return screenPos;
        }

        public Vector3 GetBounceDirection(Vector3 position, Vector3 currentDirection)
        {
            Vector3 screenPosition = _camera.WorldToViewportPoint(position);
            Vector3 newDirection = currentDirection;
            
            if (screenPosition.x < 0.01f)
            {
                newDirection.x = Mathf.Abs(newDirection.x);
            }
            if (screenPosition.x > 0.99f)
            {
                if (newDirection.x > 0.0f) newDirection.x *= -1;
            }
            if (screenPosition.y < 0.01f)
            {
                newDirection.y = Mathf.Abs(newDirection.y);
            }
            if (screenPosition.y > 0.99f)
            {
                if (newDirection.y > 0.0f) newDirection.y *= -1;
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

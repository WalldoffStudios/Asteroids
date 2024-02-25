using UnityEngine;

namespace Asteroids
{
    public static class VectorExtensions
    {
        public static Vector2 EaseInOutQuad(this Vector2 start, Vector2 end, float time, float duration = 1.0f)
        {
            // Calculate the change in position.
            Vector2 change = end - start;

            // Normalize time to half of the duration to simplify the easing calculation.
            time /= duration / 2.0f;

            // First half of the easing (accelerating from start to halfway)
            if (time < 1.0f)
            {
                return change / 2.0f * time * time + start;
            }

            // Adjust time for the second half of the easing (decelerating to the end)
            time--;

            // Second half of the easing (decelerating to the end)
            return -change / 2.0f * (time * (time - 2.0f) - 1.0f) + start;
        }
    }   
}

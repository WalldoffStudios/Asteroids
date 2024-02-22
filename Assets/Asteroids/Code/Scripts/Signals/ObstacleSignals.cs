using UnityEngine;

namespace Asteroids
{
    public class ObstacleDestroyed
    {
        public int ObjectId { get; private set; }
        public Vector2 Position { get; private set; }
        public float Size { get; private set; }

        public ObstacleDestroyed(int objectId, Vector2 position, float size)
        {
            ObjectId = objectId;
            Position = position;
            Size = size;
        }
    }
}
using UnityEngine;

namespace Asteroids
{
    public class PlayerHealthStatusChanged
    {
        public int HealthAmount { get; private set; }
        public PlayerHealthStatusChanged(int amount) => HealthAmount = amount;
    }

    public class ObjectDestroyed
    {
        public int ObjectId { get; private set; }
        public Vector2 Position { get; private set; }
        public float Size { get; private set; }

        public ObjectDestroyed(int objectId, Vector2 position, float size)
        {
            ObjectId = objectId;
            Position = position;
            Size = size;
        }
    }
}

using UnityEngine;

namespace Asteroids
{
    public class PlayerHealthStatusChangedSignal
    {
        public int HealthAmount { get; private set; }
        public PlayerHealthStatusChangedSignal(int amount) => HealthAmount = amount;
    }
    
    public class PlayerHealthInitializedSignal
    {
        public int MaxHealth { get; private set; }
        public PlayerHealthInitializedSignal(int amount) => MaxHealth = amount;
    }

    public class PlayerDiedSignal { }

    public class ObjectDestroyedSignal
    {
        public int ObjectId { get; private set; }
        public Vector2 Position { get; private set; }
        public float Size { get; private set; }

        public ObjectDestroyedSignal(int objectId, Vector2 position, float size)
        {
            ObjectId = objectId;
            Position = position;
            Size = size;
        }
    }
}

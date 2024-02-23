using UnityEngine;

namespace Asteroids
{
    public class CurrencyAmountChangedSignal
    {
        public int CurrencyAmount { get; private set; }
        public CurrencyAmountChangedSignal(int amount) => CurrencyAmount = amount;
    }

    public class CurrencySpawnSignal
    {
        public Vector2 Position { get; private set; }
        public int AmountToSpawn { get; private set; }
        public CurrencySpawnSignal(Vector2 position, int amount)
        {
            Position = position;
            AmountToSpawn = amount;
        }
    }
}
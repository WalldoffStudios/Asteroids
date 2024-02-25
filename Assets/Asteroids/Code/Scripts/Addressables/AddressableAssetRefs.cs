using System;
using UnityEngine;

namespace Asteroids
{
    [Serializable]
    public class PlayerTextureReference
    {
        public Sprite PlayerTexture { get; private set; }
        public PlayerTextureReference(Sprite texture) => PlayerTexture = texture;
    }
    
    [Serializable]
    public class CoinTextureReference
    {
        public Sprite CoinTexture { get; private set; }
        public CoinTextureReference(Sprite texture) => CoinTexture = texture;
    }
}

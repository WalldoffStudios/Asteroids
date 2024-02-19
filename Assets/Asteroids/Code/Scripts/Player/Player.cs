using UnityEngine;

namespace Asteroids
{
    public class Player : MonoBehaviour
    {
        private readonly Rigidbody2D _rigidBody;
        public SpriteRenderer Renderer { get; }
        
        public Player(Rigidbody2D rigidBody, SpriteRenderer renderer)
        {
            _rigidBody = rigidBody;
            Renderer = renderer;
        }

        private float _health;
        public bool IsDead { get; set; }
        public float Health { get; set; }
        public Vector3 LookDir => -_rigidBody.transform.right;
        
        public float Rotation
        {
            get => _rigidBody.rotation;
            set => _rigidBody.rotation = value;
        }

        public Vector3 Position
        {
            get => _rigidBody.position;
            set => _rigidBody.position = value;
        }

        public Vector3 Velocity => _rigidBody.velocity;

        public void TakeDamage(float healthLoss)
        {
            _health = Mathf.Max(0.0f, _health - healthLoss);
        }

        public void AddForce(Vector2 force) => _rigidBody.AddForce(force);
    }   
}

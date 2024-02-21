using UnityEngine;

namespace Asteroids
{
    public class Player
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
        public Vector2 LookDir => -_rigidBody.transform.right;
        
        public float Rotation
        {
            get => _rigidBody.rotation;
            set => _rigidBody.rotation = value;
        }

        public Vector2 Position
        {
            get => _rigidBody.position;
            set => _rigidBody.position = value;
        }

        public Vector2 MoveDirection
        {
            get;
            set;
        }

        public Vector2 Velocity => _rigidBody.velocity;

        public void TakeDamage(float healthLoss)
        {
            _health = Mathf.Max(0.0f, _health - healthLoss);
        }

        public void Bounce(Vector2 direction) => _rigidBody.velocity = direction;

        public void AddForce(Vector2 force)
        {
            _rigidBody.AddForce(force, ForceMode2D.Force);
            _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, 10.0f);
        }
    }   
}

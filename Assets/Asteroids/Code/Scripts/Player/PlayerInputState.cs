using UnityEngine;

namespace Asteroids
{
    public class PlayerInputState
    {
        public Vector2 LastAimPosition { get; set; }
        public bool LastShootingState { get; set; }
        public bool LastMovementState { get; set; }
        public float HorizontalInput { get; set; }
        public float VerticalInput { get; set; }
        public Vector2 MovementInput => new Vector2(HorizontalInput, VerticalInput).normalized;
        public bool IsMoving => MovementInput.sqrMagnitude > 0.0f;
    }   
}
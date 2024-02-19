using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerInputHandler : ITickable
    {
        private readonly PlayerInputState _inputState;

        public PlayerInputHandler(PlayerInputState inputState) => _inputState = inputState;
        
        public void Tick()
        {
            _inputState.HorizontalInput = Input.GetAxisRaw("Horizontal");
            _inputState.VerticalInput = Input.GetAxisRaw("Vertical");
            
            _inputState.IsFiring = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        }
    }   
}

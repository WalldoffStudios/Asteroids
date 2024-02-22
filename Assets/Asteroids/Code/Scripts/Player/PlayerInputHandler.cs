using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerInputHandler : ITickable
    {
        private readonly PlayerInputState _inputState;
        private readonly SignalBus _signalBus;

        public PlayerInputHandler(PlayerInputState inputState, SignalBus signalBus)
        {
            _inputState = inputState;
            _signalBus = signalBus;
        }

        public void Tick()
        {
            _inputState.HorizontalInput = Input.GetAxisRaw("Horizontal");
            _inputState.VerticalInput = Input.GetAxisRaw("Vertical");
            bool movedLastFrame = _inputState.LastMovementState;
            bool currentlyMoving = _inputState.IsMoving;
            if (movedLastFrame != currentlyMoving)
            {
                _signalBus.Fire(new MovementStateChangedSignal(currentlyMoving));
                _inputState.LastMovementState = currentlyMoving;
            }

            if (currentlyMoving == true)
            {
                _signalBus.Fire(new MovementUpdateSignal(_inputState.MovementInput));
            }

            if (Input.mouseScrollDelta.sqrMagnitude > 0.0f)
            {
                _signalBus.Fire(new CameraZoomSignal(Input.mouseScrollDelta.y));
            }
            
            _inputState.IsFiring = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        }
    }   
}

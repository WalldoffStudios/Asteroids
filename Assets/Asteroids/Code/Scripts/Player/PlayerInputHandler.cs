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
            CheckMovementInput();
            CheckZoomInput();
            CheckFiringInput();
            CheckForAimInput();
        }

        private void CheckMovementInput()
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
        }

        private void CheckZoomInput()
        {
            if (Input.mouseScrollDelta.sqrMagnitude > 0.0f)
            {
                _signalBus.Fire(new CameraZoomSignal(Input.mouseScrollDelta.y));
            }
        }

        private void CheckFiringInput()
        {
            bool shotLastFrame = _inputState.LastShootingState;
            bool isFiring = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
            if (shotLastFrame != isFiring)
            {
                _signalBus.Fire(new ShootInputChangedSignal(isFiring));
                _inputState.LastShootingState = isFiring;
            }
        }

        private void CheckForAimInput()
        {
            Vector2 mousePos = Input.mousePosition;
            _signalBus.Fire(new AimInputSignal(mousePos));
            
            // if (Vector2.Distance(mousePos, _inputState.LastAimPosition) > 0.05f)
            // {
            //     _signalBus.Fire(new AimInputSignal(mousePos));
            //     _inputState.LastAimPosition = mousePos;
            // }
        }
    }   
}

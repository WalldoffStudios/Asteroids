using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerInputHandler : IInitializable, ITickable, IDisposable
    {
        private readonly PlayerInputState _inputState;
        private readonly Player _player;
        private readonly SignalBus _signalBus;

        public PlayerInputHandler(PlayerInputState inputState, Player player, SignalBus signalBus)
        {
            _inputState = inputState;
            _player = player;
            _signalBus = signalBus;
        }

        private GameStates _currentState;
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(GameStateChanged);               
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(GameStateChanged);
        }

        private void GameStateChanged(GameStateChangedSignal signal)
        {
            _currentState = signal.State;
        }

        public void Tick()
        {
            if(_currentState != GameStates.Playing) return;
            if(_player.IsDead == true) return;
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
        }
    }   
}

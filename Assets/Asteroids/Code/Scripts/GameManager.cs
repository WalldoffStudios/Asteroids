using UnityEngine;
using Zenject;

namespace Asteroids
{
    public enum GameStates
    {
        Boot = 0,
        MainMenu = 1,
        WaitingToStart = 2,
        Playing = 3,
        LevelComplete = 4,
    }

    public class GameStateChangedSignal
    {
        public GameStates State { get; private set; }

        public GameStateChangedSignal(GameStates state) => State = state;
    }
    
    public class GameManager : IInitializable
    {
        private readonly SignalBus _signalBus;
        private GameStates _currentState;

        public GameManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            //ChangeState(GameStates.Boot);
            
        }

        private void ChangeState(GameStates newState)
        {
            if(_currentState == newState) return;
            _currentState = newState;
            
            _signalBus.Fire(new GameStateChangedSignal(_currentState));
            Debug.Log($"Game state changed to: {_currentState}");
        }
    }   
}

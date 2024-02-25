using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public enum GameStates
    {
        Boot = 0,
        GameSceneLoading = 1,
        WaitingToStart = 2,
        Playing = 3,
        LevelComplete = 4,
    }
    
    public class GameManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private GameStates _currentState;

        public GameManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameSceneInitializedSignal>(GameSceneInitialized);
            ChangeState(GameStates.Boot);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameSceneInitializedSignal>(GameSceneInitialized);
        }

        //is called when scene context is finished with installs
        private void GameSceneInitialized(GameSceneInitializedSignal signal)
        {
            ChangeState(GameStates.WaitingToStart);
            MonoBehaviourHelper.Instance.InvokeWithDelay(StartGame, 5.0f);
        }

        private void StartGame()
        {
            ChangeState(GameStates.Playing);
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

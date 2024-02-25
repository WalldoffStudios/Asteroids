using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            //ChangeState(GameStates.Boot);
            Debug.Log("GameManager was initialized");
            _signalBus.Subscribe<AssetsBoundSignal>(DownloadedAssets);
            ChangeState(GameStates.Boot);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<AssetsBoundSignal>(DownloadedAssets);
        }

        private void DownloadedAssets()
        {
            //ChangeState(GameStates.Boot);
            var loadOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            loadOperation.completed += SceneLoaded;
        }

        private void SceneLoaded(AsyncOperation operation)
        {
            ChangeState(GameStates.WaitingToStart);
            
            operation.completed -= SceneLoaded;
            
            MonoBehaviourHelper.Instance.InvokeWithDelay(StartGame, 5.0f);
            //MonoBehaviourHelper.Instance.GameCountdown();
        }

        private void StartGame()
        {
            Debug.Log("Starting playing state");
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

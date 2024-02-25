using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            //ChangeState(GameStates.Boot);
            Debug.Log("GameManager was initialized");
            //_signalBus.Subscribe<AssetsBoundSignal>(DownloadedAssets);
            _signalBus.Subscribe<GameSceneInitializedSignal>(GameSceneInitialized);
            ChangeState(GameStates.Boot);
        }

        public void Dispose()
        {
            //_signalBus.Unsubscribe<AssetsBoundSignal>(DownloadedAssets);
            _signalBus.Unsubscribe<GameSceneInitializedSignal>(GameSceneInitialized);
        }

        private void DownloadedAssets()
        {
            //ChangeState(GameStates.Boot);
            var loadOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            //loadOperation.completed += SceneLoaded;
        }

        private void SceneLoaded(AsyncOperation operation)
        {
            // ChangeState(GameStates.GameSceneLoading);
            
            //operation.completed -= SceneLoaded;
            
            // MonoBehaviourHelper.Instance.InvokeWithDelay(StartGame, 5.0f);
            //MonoBehaviourHelper.Instance.GameCountdown();
        }

        //is called when scene context is finished with installs
        private void GameSceneInitialized(GameSceneInitializedSignal signal)
        {
            ChangeState(GameStates.WaitingToStart);
            MonoBehaviourHelper.Instance.InvokeWithDelay(StartGame, 5.0f);
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

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Asteroids
{
    public class SceneLoadCompletedSignal
    {
        public int SceneIndex { get; private set; }
        public SceneLoadCompletedSignal(int sceneIndex) => SceneIndex = sceneIndex;
    }

    public class TriggerSceneChangeSignal
    {
        public int SceneIndex { get; private set; }
        public TriggerSceneChangeSignal(int sceneIndex) => SceneIndex = sceneIndex;
    }
    
    public class SceneHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        public SceneHandler(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private int _currentSceneIndex;
        
        public void Initialize()
        {
            _signalBus.Subscribe<TriggerSceneChangeSignal>(ChangeScene);
            _signalBus.Subscribe<PlayerDiedSignal>(PlayerLimbo);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<TriggerSceneChangeSignal>(ChangeScene);
            _signalBus.Unsubscribe<PlayerDiedSignal>(PlayerLimbo);
        }

        //Player stays in limbo 5 seconds after dying, then changes scene
        private void PlayerLimbo()
        {
            MonoBehaviourHelper.Instance.InvokeWithDelay(PlayerDied, 5.0f);
        }

        private void PlayerDied()
        {
            LoadScene(1, false);
        }

        private void ChangeScene(TriggerSceneChangeSignal signal)
        {
            LoadScene(signal.SceneIndex, true);
        }

        private void LoadScene(int index, bool async)
        {
            if (async == true)
            {
                var sceneLoadOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
                sceneLoadOperation.completed += SceneLoadCompleted;
            }
            else
            {
                SceneManager.LoadScene(index);
            }
            _currentSceneIndex = index;
        }

        private void SceneLoadCompleted(AsyncOperation operation)
        {
            Debug.Log("Finished loading scene");
            
            //todo: nothing listens to this one atm so I dont want to fire it
            //_signalBus.Fire(new SceneLoadCompletedSignal(_currentSceneIndex));
        }
    }   
}

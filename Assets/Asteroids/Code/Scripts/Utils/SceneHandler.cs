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
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<TriggerSceneChangeSignal>(ChangeScene);
        }

        private void ChangeScene(TriggerSceneChangeSignal signal)
        {
            var sceneLoadOperation = SceneManager.LoadSceneAsync(signal.SceneIndex, LoadSceneMode.Single);
            sceneLoadOperation.completed += SceneLoadCompleted;
            _currentSceneIndex = signal.SceneIndex;
        }

        private void SceneLoadCompleted(AsyncOperation operation)
        {
            _signalBus.Fire(new SceneLoadCompletedSignal(_currentSceneIndex));
        }
    }   
}

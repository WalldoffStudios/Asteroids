using System;
using TMPro;
using Zenject;

namespace Asteroids
{
    public class UIGameStatusHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly TextMeshProUGUI _countDownLabelText;
        private readonly TextMeshProUGUI _countDownText;

        public UIGameStatusHandler(UICenterCanvas centerCanvas, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _countDownLabelText = centerCanvas.CountDownLabelText;
            _countDownText = centerCanvas.CountDownText;
        }
        
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
            switch (signal.State)
            {
                case GameStates.Boot:
                    break;
                case GameStates.GameSceneLoading:
                    break;
                case GameStates.WaitingToStart:
                    StartGameCountDown();        
                    break;
                case GameStates.Playing:
                    DisableCountdown();
                    break;
                case GameStates.LevelComplete:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartGameCountDown()
        {
            _countDownLabelText.gameObject.SetActive(true);
            _countDownText.gameObject.SetActive(true);

            _countDownLabelText.text = "Game starting in:";
            
            //todo: fix the hardcoded countdown timer value here
            MonoBehaviourHelper.Instance.GameCountdown(_countDownText, 5.0f);
        }

        private void DisableCountdown()
        {
            _countDownLabelText.text = "";
            _countDownText.text = "";
            _countDownLabelText.gameObject.SetActive(false);
            _countDownText.gameObject.SetActive(false);
        }
    }
}
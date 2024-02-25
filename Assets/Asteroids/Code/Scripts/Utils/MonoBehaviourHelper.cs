using System.Collections;
using System;
using TMPro;
using UnityEngine;

namespace Asteroids
{
    public class MonoBehaviourHelper : MonoBehaviour
    {
        // Singleton instance
        public static MonoBehaviourHelper Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optional: persist across scenes
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void InvokeWithDelay(Action method, float delay)
        {
            StartCoroutine(InvokeRoutine(method, delay));
        }

        private IEnumerator InvokeRoutine(Action method, float delay)
        {
            yield return new WaitForSeconds(delay);
            method.Invoke();
        }

        public void GameCountdown(TextMeshProUGUI countDownText, float time, Action callbackOnEnd = null)
        {
            StartCoroutine(GameCountdownRoutine(countDownText, time, callbackOnEnd));
        }

        private IEnumerator GameCountdownRoutine(TextMeshProUGUI countDownText, float time, Action callbackOnEnd = null)
        {
            //countDownText.text = $"Game starting in {time} seconds";
            int secondsRounded = Mathf.RoundToInt(time);
            for (int i = 0; i < secondsRounded; i++)
            {
                countDownText.text = $"{time - i} seconds";
                yield return new WaitForSeconds(1.0f);
            }
            
            callbackOnEnd?.Invoke();
        }
    }   
}

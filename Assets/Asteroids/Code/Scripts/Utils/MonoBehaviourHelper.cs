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
            int secondsRounded = Mathf.RoundToInt(time);
            for (int i = 0; i < secondsRounded; i++)
            {
                countDownText.text = $"{time - i} seconds";
                yield return new WaitForSeconds(1.0f);
            }
            
            callbackOnEnd?.Invoke();
        }

        public void TweenScale(Transform tweenTransform, float startScale, float targetScale, float time)
        {
            StartCoroutine(TweenScaleRoutine(tweenTransform, startScale, targetScale, time));
        }

        private IEnumerator TweenScaleRoutine(Transform tweenTransform, float startScale, float targetScale, float time)
        {
            float tweenTimer = 0.0f;
            float speedMultiplier = 1.0f / time;
            
            while (tweenTimer < 1.0f)
            {
                tweenTimer += Time.deltaTime * speedMultiplier;
                float scaleMultiplier = Mathf.Lerp(startScale, targetScale, tweenTimer);
                if(tweenTransform != null) tweenTransform.localScale = Vector2.one * scaleMultiplier;
                yield return null;
            }

            tweenTimer = 0.0f;
            while (tweenTimer < 1.0f)
            {
                tweenTimer += Time.deltaTime * speedMultiplier;
                float scaleMultiplier = Mathf.Lerp(targetScale, startScale, tweenTimer);
                if(tweenTransform != null) tweenTransform.localScale = Vector2.one * scaleMultiplier;
                yield return null;
            }
            tweenTransform.localScale = Vector2.one * startScale;
        }
    }   
}

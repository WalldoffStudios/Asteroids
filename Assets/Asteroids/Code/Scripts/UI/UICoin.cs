using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public struct CoinSpawnParams
    {
        public float PopupSpeed { get; }
        public float CollectSpeed { get; }
        public Vector2 TargetPosition { get; }

        public CoinSpawnParams(float popupSpeed, float collectSpeed, Vector2 targetPosition)
        {
            PopupSpeed = popupSpeed;
            CollectSpeed = collectSpeed;
            TargetPosition = targetPosition;
        }
    }
    public class UICoin : MonoBehaviour, IPoolable<CoinSpawnParams, IMemoryPool>
    {
        [SerializeField] private RectTransform rectTransform;
        
        [Inject]
        private SignalBus _signalBus;
        
        private float _popupSpeed;
        private float _collectSpeed;
        private Vector2 _targetPosition;
        private IMemoryPool _pool;

        public void OnSpawned(CoinSpawnParams spawnParams, IMemoryPool pool)
        {
            _popupSpeed = spawnParams.PopupSpeed;
            _collectSpeed = spawnParams.CollectSpeed;
            _targetPosition = spawnParams.TargetPosition;
            _pool = pool;
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }

        public void PopupCoin()
        {
            // transform.localScale = Vector2.zero;
            // DOTween.Sequence().Append(transform.DOScale(Vector3.one, _popupSpeed * 0.5f).SetEase(Ease.InOutQuad));
            // Vector2 randomDirection = (Random.insideUnitCircle - (Vector2)transform.position).normalized; 
            // Vector2 randomPosition = (Vector2)transform.position + randomDirection;
            // DOTween.Sequence().Append(rectTransform.DOLocalMove(randomPosition, _popupSpeed).SetEase(Ease.InOutQuad).OnComplete(CollectCoin));
            StartCoroutine(ScaleLerpRoutine());
        }

        // private void CollectCoin()
        // {
        //     DOTween.Sequence()
        //         .Append(rectTransform
        //             .DOLocalMove(_targetPosition, _collectSpeed)
        //             .SetEase(Ease.InOutQuad)).OnComplete(() =>
        //             {
        //                 _signalBus.Fire(new CurrencyAmountChangedSignal(1));
        //                 Debug.Log("Should despawn coin");
        //                 _pool.Despawn(this);
        //             });
        // }
        
        private IEnumerator ScaleLerpRoutine()
        {
            float scaleTimer = 0.0f;
            Vector3 startScale = Vector3.zero;
            Vector3 targetScale = Vector3.one;

            Vector3 startPos = rectTransform.localPosition;
            float randomXOffset = Random.Range(-100.0f, 100.0f);
            float randomYOffset = Random.Range(-100.0f, 100.0f);
            Vector3 targetPos = startPos;
            targetPos.x += randomXOffset;
            targetPos.y += randomYOffset;
            
            while (scaleTimer < 1.0f)
            {
                scaleTimer += Time.deltaTime * _popupSpeed;
                transform.localScale = EaseInOutQuad(startScale, targetScale, scaleTimer, .9f);
                rectTransform.localPosition = EaseInOutQuad(startPos, targetPos, scaleTimer);
                yield return null;
            }
            StartCoroutine(LerpToCorner());
        }
        
        public static Vector3 EaseInOutQuad(Vector3 a, Vector3 b, float t, float d = 1f)
        {
            // quadratic easing in/out - acceleration until halfway, then deceleration
            Vector3 c = b - a;
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + a;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + a;
        }

        private IEnumerator LerpToCorner()
        {
            float lerptimer = 0.0f;
            Vector3 startPos = rectTransform.localPosition;
        
            while (lerptimer < 1.0f)
            {
                lerptimer += Time.deltaTime * _collectSpeed;
                if (rectTransform != null)
                {
                    rectTransform.localPosition = EaseInOutQuad(startPos, _targetPosition, lerptimer);   
                }
                else
                {
                    _pool.Despawn(this);
                    yield break;
                }
                yield return null;
            }

            _signalBus.Fire(new CurrencyAmountChangedSignal(1));
            Debug.Log("Should despawn coin");
            _pool.Despawn(this);
        }
        
        public class Factory : PlaceholderFactory<CoinSpawnParams, UICoin>
        {
        }
    }
}
using System.Collections;
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
            StartCoroutine(ScaleLerpRoutine());
        }
        
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
        
        //todo: move to static extension class
        private static Vector3 EaseInOutQuad(Vector3 start, Vector3 end, float time, float duration = 1f)
        {
            // Calculate the change in position.
            Vector3 change = end - start;
    
            // Normalize time to half of the duration to simplify the easing calculation.
            time /= duration / 2;
    
            // First half of the easing (accelerating from start to halfway)
            if (time < 1)
            {
                return change / 2 * time * time + start;
            }
    
            // Adjust time for the second half of the easing (decelerating to the end)
            time--;
    
            // Second half of the easing (decelerating to the end)
            return -change / 2 * (time * (time - 2) - 1) + start;
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
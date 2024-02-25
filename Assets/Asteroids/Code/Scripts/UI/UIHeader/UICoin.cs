using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private Image coinImage;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus, CoinTextureReference coinTexture)
        {
            _signalBus = signalBus;
            coinImage.sprite = coinTexture.CoinTexture;
        }
        
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
            Vector2 startScale = Vector3.zero;
            Vector2 targetScale = Vector3.one;

            Vector2 startPos = rectTransform.localPosition;
            float randomXOffset = Random.Range(-100.0f, 100.0f);
            float randomYOffset = Random.Range(-100.0f, 100.0f);
            Vector2 targetPos = startPos;
            targetPos.x += randomXOffset;
            targetPos.y += randomYOffset;
            
            while (scaleTimer < 1.0f)
            {
                scaleTimer += Time.deltaTime * _popupSpeed;
                transform.localScale = startScale.EaseInOutQuad(targetScale, scaleTimer, 0.9f);
                rectTransform.localPosition = startPos.EaseInOutQuad(targetPos, scaleTimer);
                yield return null;
            }
            StartCoroutine(LerpToCorner());
        }

        private IEnumerator LerpToCorner()
        {
            float lerptimer = 0.0f;
            Vector2 startPos = rectTransform.localPosition;
        
            while (lerptimer < 1.0f)
            {
                lerptimer += Time.deltaTime * _collectSpeed;
                if (rectTransform != null)
                {
                    rectTransform.localPosition = startPos.EaseInOutQuad(_targetPosition, lerptimer);   
                }
                else
                {
                    _pool.Despawn(this);
                    yield break;
                }
                yield return null;
            }

            _signalBus.Fire(new CurrencyAmountChangedSignal(1));
            _pool.Despawn(this);
        }
        
        public class Factory : PlaceholderFactory<CoinSpawnParams, UICoin>
        {
        }
    }
}
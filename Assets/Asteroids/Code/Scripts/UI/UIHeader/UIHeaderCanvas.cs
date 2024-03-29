using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
    public class UIHeaderCanvas : MonoBehaviour
    {
        [field: SerializeField] public RectTransform HeaderContainerRect { get; private set; } = null;
        
        [field: SerializeField] public GameObject HealthContainer { get; private set; } = null;
        [field: SerializeField] public TextMeshProUGUI HealthText { get; private set; } = null;
        [field: SerializeField] public RectTransform HealthRect { get; private set; } = null;
        
        [field: SerializeField] public TextMeshProUGUI CurrencyText { get; private set; } = null;
        [field: SerializeField] public RectTransform CurrencyImageRect { get; private set; } = null;
        [field: SerializeField] public Image CurrencyImage { get; private set; } = null;
    }   
}
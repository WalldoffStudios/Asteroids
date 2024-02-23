using TMPro;
using UnityEngine;

namespace Asteroids
{
    public class UIHeaderCanvas : MonoBehaviour
    {
        //todo: this will act as the "facade" for the whole header canvas
        //todo: setup sub classes to handle the different UI components
        [field: SerializeField] public RectTransform HeaderContainerRect { get; private set; } = null;
        [field: SerializeField] public TextMeshProUGUI HealthText { get; private set; } = null;
        [field: SerializeField] public RectTransform HealthRect { get; private set; } = null;
        
        [field: SerializeField] public TextMeshProUGUI CurrencyText { get; private set; } = null;
        [field: SerializeField] public RectTransform CurrencyImageRect { get; private set; } = null;
    }   
}
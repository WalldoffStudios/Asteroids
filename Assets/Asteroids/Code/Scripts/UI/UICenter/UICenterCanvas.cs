using TMPro;
using UnityEngine;

namespace Asteroids
{
    public class UICenterCanvas : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI CountDownLabelText { get; private set; } = null;
        [field: SerializeField] public TextMeshProUGUI CountDownText { get; private set; } = null;
    }   
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class ResourceItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI amountText;
        public Image inputOutputIndicator;
        
        public void Initialize(ResourceType resource, float amount, bool isInput)
        {
            nameText.text = resource.name;
            amountText.text = amount.ToString("F1");
            inputOutputIndicator.color = isInput ? Color.red : Color.green; // Red for input, green for output
        }
    }
} 
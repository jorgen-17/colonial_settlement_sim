using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class InventoryItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI amountText;
        
        public void Initialize(ResourceType resource, float amount)
        {
            nameText.text = resource.name;
            amountText.text = amount.ToString("F1");
        }
    }
} 
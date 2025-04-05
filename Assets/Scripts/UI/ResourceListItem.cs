using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class ResourceListItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI valueText;
        public TextMeshProUGUI weightText;
        public TextMeshProUGUI typeText;

        private ResourceType resource;

        public void Initialize(ResourceType resource)
        {
            this.resource = resource;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (resource == null) return;

            nameText.text = resource.name;
            descriptionText.text = resource.description;
            valueText.text = $"Value: {resource.baseValue:F1}";
            weightText.text = $"Weight: {resource.weight:F1}";
            typeText.text = resource.isRawMaterial ? "Raw Material" : "Processed";
        }
    }
}
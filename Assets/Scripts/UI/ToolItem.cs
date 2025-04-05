using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class ToolItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        
        public void Initialize(ResourceType tool)
        {
            nameText.text = tool.name;
            descriptionText.text = tool.description;
        }
    }
} 
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class WorkRouteItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI stepText;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI typeText;
        public Image currentIndicator;
        
        public void Initialize(WorkArea workArea, int step, bool isCurrent)
        {
            stepText.text = $"Step {step}";
            nameText.text = workArea.areaName;
            typeText.text = workArea.areaType.ToString();
            currentIndicator.gameObject.SetActive(isCurrent);
        }
    }
} 
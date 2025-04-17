using UnityEngine;
using TMPro;
using css.core;

namespace css.ui
{
    public class WorkAreaUI : MonoBehaviour
    {
        [Header("Text Settings")]
        public Color textColor = Color.black;
        public float fontSize = 2f;
        public Vector2 textDimensions = new Vector2(10, 2);

        private WorkArea workArea;
        private TextMeshPro nameText;
        private GameObject nameTextObj;

        private void Start()
        {
            workArea = GetComponent<WorkArea>();
            if (workArea == null)
            {
                Debug.LogError("WorkAreaUI requires a WorkArea component on the same GameObject");
                return;
            }

            CreateNameText();
        }

        private void CreateNameText()
        {
            // Create a new GameObject for the text
            nameTextObj = new GameObject("AreaNameText");
            nameTextObj.transform.SetParent(transform);
            
            // Position the text above the work area
            nameTextObj.transform.localPosition = new Vector3(0, 1.5f, 0);
            nameTextObj.transform.localRotation = Quaternion.Euler(90, 0, 0); // Face up
            
            // Add TextMeshPro component
            nameText = nameTextObj.AddComponent<TextMeshPro>();
            nameText.text = workArea.areaName;
            nameText.alignment = TextAlignmentOptions.Center;
            nameText.fontSize = fontSize;
            nameText.color = textColor;
            
            // Adjust the text rect transform
            RectTransform rectTransform = nameText.GetComponent<RectTransform>();
            rectTransform.sizeDelta = textDimensions;
            rectTransform.localScale = new Vector3(1, 1, 1);
        }

        private void Update()
        {
            if (nameText != null && workArea != null)
            {
                // Update text if area name changes
                nameText.text = workArea.areaName;
            }
        }
    }
} 
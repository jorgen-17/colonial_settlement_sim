using UnityEngine;
using TMPro;
using css.core;

namespace css.ui
{
    public class NPCUI : MonoBehaviour
    {
        [Header("Text Settings")]
        public Color textColor = Color.black;
        public float fontSize = 1.5f;
        public Vector2 textDimensions = new Vector2(5, 1);
        public float heightOffset = 2f; // How high above the NPC the text should float

        private NPC npc;
        private TextMeshPro nameText;
        private GameObject nameTextObj;

        private void Start()
        {
            npc = GetComponent<NPC>();
            if (npc == null)
            {
                Debug.LogError("NPCUI requires an NPC component on the same GameObject");
                return;
            }

            CreateNameText();
        }

        private void CreateNameText()
        {
            // Create a new GameObject for the text
            nameTextObj = new GameObject("NPCNameText");
            nameTextObj.transform.SetParent(transform);
            
            // Position the text above the NPC
            nameTextObj.transform.localPosition = new Vector3(0, heightOffset, 0);
            nameTextObj.transform.localRotation = Quaternion.Euler(0, 0, 0); // Face forward
            
            // Add TextMeshPro component
            nameText = nameTextObj.AddComponent<TextMeshPro>();
            nameText.text = npc.npcName;
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
            if (nameText != null && npc != null)
            {
                // Update text if NPC name changes
                nameText.text = npc.npcName;
                
                // Make the text always face the camera
                nameTextObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
} 
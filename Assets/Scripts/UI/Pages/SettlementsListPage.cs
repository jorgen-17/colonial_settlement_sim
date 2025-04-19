using UnityEngine;
using TMPro;
using css.core;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace css.ui
{
    public class SettlementsListPage : MonoBehaviour, MenuPage
    {
        [Header("UI References")]
        public GameObject settlementsListPanel;
        public TextMeshProUGUI settlementsTitle;
        public List<GameObject> settlementButtons = new List<GameObject>();
        private Settlement selectedSettlement;
        private Transform parentTransform;

        public void Update()
        {
            UpdateSettlementsList();
        }

        public void HandleMouseClick(Vector2 mousePosition)
        {
            // Check if mouse is over any settlement
            foreach (GameObject button in settlementButtons)
            {
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                // Convert mouse position to local space of the settlement button
                Vector2 localMousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform, mousePosition, null, out localMousePos);
                
                // Check if the mouse is inside the settlement button
                if (rectTransform.rect.Contains(localMousePos))
                {
                    try
                    {
                        // Get the settlement by ID using Guid
                        string settlementIdString = button.name.Replace("Settlement_", "");
                        Guid settlementId = Guid.Parse(settlementIdString);
                        
                        // Use the GameManager method to find the settlement
                        Settlement clickedSettlement = GameManager.Instance.settlements.Find(s => s.id == settlementId);
                        
                        if (clickedSettlement != null)
                        {
                            selectedSettlement = clickedSettlement;
                            Debug.Log($"Clicked on settlement: {clickedSettlement.settlementName}, ID: {clickedSettlement.id}");
                            break;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Debug.LogError($"Failed to parse settlement ID: {ex.Message}");
                    }
                }
            }
        }

        public void Initialize(Transform parent)
        {
            parentTransform = parent;
            CreateSettlementsListPanel();
        }

        public void CreateSettlementsListPanel()
        {
            if (settlementsListPanel == null)
            {
                settlementsListPanel = new GameObject("SettlementsListPanel");
                settlementsListPanel.transform.SetParent(parentTransform);
                
                // Add RectTransform
                RectTransform rectTransform = settlementsListPanel.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = new Vector2(500, 200);
                rectTransform.anchoredPosition = new Vector2(500, -200);

                // Add GraphicRaycaster to handle UI events
                settlementsListPanel.AddComponent<GraphicRaycaster>();

                // Create settlements title
                GameObject titleObj = new GameObject("SettlementsTitle");
                titleObj.transform.SetParent(settlementsListPanel.transform);
                
                RectTransform titleRect = titleObj.AddComponent<RectTransform>();
                titleRect.anchorMin = new Vector2(0f, 1f);
                titleRect.anchorMax = new Vector2(0f, 1f);
                titleRect.sizeDelta = new Vector2(480, 40);
                titleRect.anchoredPosition = new Vector2(10, -10);

                settlementsTitle = titleObj.AddComponent<TextMeshProUGUI>();
                settlementsTitle.text = "Settlements";
                settlementsTitle.alignment = TextAlignmentOptions.Left;
                settlementsTitle.fontSize = 64;
                settlementsTitle.color = Color.white;
            }
        }

        private void UpdateSettlementsList()
        {
            if (selectedSettlement != null)
            {
                settlementsTitle.text = $"Selected: {selectedSettlement.settlementName}";
            }
            
            if (GameManager.Instance == null) return;

            // Clear existing settlement buttons
            foreach (var button in settlementButtons)
            {
                Destroy(button);
            }
            settlementButtons.Clear();

            // Create new settlement buttons
            float yOffset = -100f;
            foreach (var settlement in GameManager.Instance.settlements)
            {
                GameObject buttonObj = new GameObject($"Settlement_{settlement.id}");
                buttonObj.transform.SetParent(settlementsListPanel.transform);
                
                // Add RectTransform
                RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
                buttonRect.anchorMin = new Vector2(0f, 1f);
                buttonRect.anchorMax = new Vector2(0f, 1f);
                buttonRect.sizeDelta = new Vector2(800, 30);
                buttonRect.anchoredPosition = new Vector2(100, yOffset);

                // Add Image component with visible background
                Image image = buttonObj.AddComponent<Image>();
                image.color = new Color(0.3f, 0.3f, 0.3f, 0.9f); // Very visible background

                // Create text container
                GameObject textObj = new GameObject("Text");
                textObj.transform.SetParent(buttonObj.transform);
                
                // Add RectTransform to text
                RectTransform textRect = textObj.AddComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.sizeDelta = Vector2.zero;
                textRect.offsetMin = Vector2.zero;
                textRect.offsetMax = Vector2.zero;

                // Add TextMeshProUGUI
                TextMeshProUGUI settlementText = textObj.AddComponent<TextMeshProUGUI>();
                settlementText.text = $"{settlement.settlementName} Est({settlement.foundedDate}) (Pop: {settlement.Population})";
                settlementText.alignment = TextAlignmentOptions.Left;
                settlementText.fontSize = 48;
                settlementText.color = Color.white;

                // Store the settlement button
                settlementButtons.Add(buttonObj);
                yOffset -= 50f;
            }
        }
    }
} 
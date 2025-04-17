using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;
using System.Collections.Generic;

namespace css.ui
{
    public class MenuUI : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject menuPanel;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI dayText;
        public GameObject settlementsListPanel;
        public TextMeshProUGUI settlementsTitle;
        public List<TextMeshProUGUI> settlementTexts = new List<TextMeshProUGUI>();

        private void Awake()
        {
            // Create menu panel if it doesn't exist
            if (menuPanel == null)
            {
                menuPanel = new GameObject("MenuPanel");
                menuPanel.transform.SetParent(transform);
                
                // Add RectTransform
                RectTransform rectTransform = menuPanel.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(1f, 1f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
                rectTransform.sizeDelta = new Vector2(300, 100);
                rectTransform.anchoredPosition = new Vector2(-150, -10);

                // Add Image component for background
                UnityEngine.UI.Image image = menuPanel.AddComponent<UnityEngine.UI.Image>();
                image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

                // Create Canvas
                Canvas canvas = menuPanel.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                // Add CanvasScaler
                CanvasScaler scaler = menuPanel.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                // Add GraphicRaycaster
                menuPanel.AddComponent<GraphicRaycaster>();
            }

            // Create time text if it doesn't exist
            if (timeText == null)
            {
                GameObject timeTextObj = new GameObject("TimeText");
                timeTextObj.transform.SetParent(menuPanel.transform);
                
                // Add RectTransform
                RectTransform timeRect = timeTextObj.AddComponent<RectTransform>();
                timeRect.anchorMin = new Vector2(1f, 1f);
                timeRect.anchorMax = new Vector2(1f, 1f);
                timeRect.sizeDelta = new Vector2(280, 40);
                timeRect.anchoredPosition = new Vector2(-160, -10);

                // Add TextMeshPro component
                timeText = timeTextObj.AddComponent<TextMeshProUGUI>();
                timeText.alignment = TextAlignmentOptions.Right;
                timeText.fontSize = 24;
                timeText.color = Color.white;
            }

            // Create day text if it doesn't exist
            if (dayText == null)
            {
                GameObject dayTextObj = new GameObject("DayText");
                dayTextObj.transform.SetParent(menuPanel.transform);
                
                // Add RectTransform
                RectTransform dayRect = dayTextObj.AddComponent<RectTransform>();
                dayRect.anchorMin = new Vector2(1f, 1f);
                dayRect.anchorMax = new Vector2(1f, 1f);
                dayRect.sizeDelta = new Vector2(280, 40);
                dayRect.anchoredPosition = new Vector2(-160, -60);

                // Add TextMeshPro component
                dayText = dayTextObj.AddComponent<TextMeshProUGUI>();
                dayText.alignment = TextAlignmentOptions.Right;
                dayText.fontSize = 24;
                dayText.color = Color.white;
            }

            // Create settlements list panel if it doesn't exist
            if (settlementsListPanel == null)
            {
                settlementsListPanel = new GameObject("SettlementsListPanel");
                settlementsListPanel.transform.SetParent(menuPanel.transform);
                
                // Add RectTransform
                RectTransform rectTransform = settlementsListPanel.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = new Vector2(500, 200);
                rectTransform.anchoredPosition = new Vector2(500, -200);

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

        private void Update()
        {
            UpdateSettlementsList();
        }

        private void UpdateSettlementsList()
        {
            if (GameManager.Instance == null) return;

            // Clear existing settlement texts
            foreach (var text in settlementTexts)
            {
                Destroy(text.gameObject);
            }
            settlementTexts.Clear();

            // Create new settlement texts
            float yOffset = -100f; // Start below the title
            foreach (var settlement in GameManager.Instance.settlements)
            {
                GameObject settlementObj = new GameObject($"Settlement_{settlement.settlementName}");
                settlementObj.transform.SetParent(settlementsListPanel.transform);
                
                RectTransform rectTransform = settlementObj.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = new Vector2(800, 30);
                rectTransform.anchoredPosition = new Vector2(100, yOffset);

                TextMeshProUGUI settlementText = settlementObj.AddComponent<TextMeshProUGUI>();
                settlementText.text = $"{settlement.settlementName} Est({settlement.foundedDate}) (Pop: {settlement.Population})";
                settlementText.alignment = TextAlignmentOptions.Left;
                settlementText.fontSize = 48;
                settlementText.color = Color.white;

                settlementTexts.Add(settlementText);
                yOffset -= 50f; // Move down for next settlement
            }
        }
    }
} 
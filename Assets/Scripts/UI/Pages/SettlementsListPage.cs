using UnityEngine;
using TMPro;
using css.core;
using System.Collections.Generic;

namespace css.ui
{
    public class SettlementsListPage : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject settlementsListPanel;
        public TextMeshProUGUI settlementsTitle;
        public List<TextMeshProUGUI> settlementTexts = new List<TextMeshProUGUI>();

        private Transform parentTransform;

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

        public void Update()
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
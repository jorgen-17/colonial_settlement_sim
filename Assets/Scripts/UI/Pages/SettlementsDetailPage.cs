using UnityEngine;
using TMPro;
using css.core;
using System.Collections.Generic;

namespace css.ui
{
    public class SettlementsDetailPage : MonoBehaviour, MenuPage
    {
        [Header("UI References")]
        public GameObject detailPanel;
        public TextMeshProUGUI settlementTitle;
        public TextMeshProUGUI settlementDetails;
        public GameObject npcsPanel;
        public GameObject workAreasPanel;
        public TextMeshProUGUI npcsTitle;
        public TextMeshProUGUI workAreasTitle;
        public List<TextMeshProUGUI> npcTexts = new List<TextMeshProUGUI>();
        public List<TextMeshProUGUI> workAreaTexts = new List<TextMeshProUGUI>();

        private Transform parentTransform;
        private Settlement currentSettlement;

        public void Initialize(Transform parent)
        {
            parentTransform = parent;
            CreateDetailPanel();
            detailPanel.SetActive(false); // Hide the panel by default
        }

        public void SetSettlement(Settlement settlement)
        {
            currentSettlement = settlement;
            UpdateSettlementDetails();
            UpdateNPCsList();
            UpdateWorkAreasList();
        }

        private void CreateDetailPanel()
        {
            if (detailPanel == null)
            {
                detailPanel = new GameObject("DetailPanel");
                detailPanel.transform.SetParent(parentTransform);
                
                // Add RectTransform
                RectTransform rectTransform = detailPanel.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = new Vector2(1000, 800);
                rectTransform.anchoredPosition = new Vector2(650, -500);

                // Create settlement title
                GameObject titleObj = new GameObject("SettlementTitle");
                titleObj.transform.SetParent(detailPanel.transform);
                
                RectTransform titleRect = titleObj.AddComponent<RectTransform>();
                titleRect.anchorMin = new Vector2(0f, 1f);
                titleRect.anchorMax = new Vector2(0f, 1f);
                titleRect.sizeDelta = new Vector2(900, 60);
                titleRect.anchoredPosition = new Vector2(350, 0);

                settlementTitle = titleObj.AddComponent<TextMeshProUGUI>();
                settlementTitle.alignment = TextAlignmentOptions.Left;
                settlementTitle.fontSize = 48;
                settlementTitle.color = Color.white;

                // Create settlement details
                GameObject detailsObj = new GameObject("SettlementDetails");
                detailsObj.transform.SetParent(detailPanel.transform);
                
                RectTransform detailsRect = detailsObj.AddComponent<RectTransform>();
                detailsRect.anchorMin = new Vector2(0f, 1f);
                detailsRect.anchorMax = new Vector2(0f, 1f);
                detailsRect.sizeDelta = new Vector2(900, 100);
                detailsRect.anchoredPosition = new Vector2(400, -100);

                settlementDetails = detailsObj.AddComponent<TextMeshProUGUI>();
                settlementDetails.alignment = TextAlignmentOptions.Left;
                settlementDetails.fontSize = 24;
                settlementDetails.color = Color.white;

                // Create NPCs panel
                npcsPanel = new GameObject("NPCsPanel");
                npcsPanel.transform.SetParent(detailPanel.transform);
                
                RectTransform npcsRect = npcsPanel.AddComponent<RectTransform>();
                npcsRect.anchorMin = new Vector2(0f, 1f);
                npcsRect.anchorMax = new Vector2(0.5f, 1f);
                npcsRect.sizeDelta = new Vector2(400, 600);
                npcsRect.anchoredPosition = new Vector2(300, -500);

                // Create NPCs title
                GameObject npcsTitleObj = new GameObject("NPCsTitle");
                npcsTitleObj.transform.SetParent(npcsPanel.transform);
                
                RectTransform npcsTitleRect = npcsTitleObj.AddComponent<RectTransform>();
                npcsTitleRect.anchorMin = new Vector2(0f, 1f);
                npcsTitleRect.anchorMax = new Vector2(0f, 1f);
                npcsTitleRect.sizeDelta = new Vector2(380, 40);
                npcsTitleRect.anchoredPosition = new Vector2(10, -10);

                npcsTitle = npcsTitleObj.AddComponent<TextMeshProUGUI>();
                npcsTitle.text = "NPCs";
                npcsTitle.alignment = TextAlignmentOptions.Left;
                npcsTitle.fontSize = 36;
                npcsTitle.color = Color.white;

                // Create Work Areas panel
                workAreasPanel = new GameObject("WorkAreasPanel");
                workAreasPanel.transform.SetParent(detailPanel.transform);
                
                RectTransform workAreasRect = workAreasPanel.AddComponent<RectTransform>();
                workAreasRect.anchorMin = new Vector2(0.5f, 1f);
                workAreasRect.anchorMax = new Vector2(1f, 1f);
                workAreasRect.sizeDelta = new Vector2(400, 600);
                workAreasRect.anchoredPosition = new Vector2(650, -500);

                // Create Work Areas title
                GameObject workAreasTitleObj = new GameObject("WorkAreasTitle");
                workAreasTitleObj.transform.SetParent(workAreasPanel.transform);
                
                RectTransform workAreasTitleRect = workAreasTitleObj.AddComponent<RectTransform>();
                workAreasTitleRect.anchorMin = new Vector2(0f, 1f);
                workAreasTitleRect.anchorMax = new Vector2(0f, 1f);
                workAreasTitleRect.sizeDelta = new Vector2(380, 40);
                workAreasTitleRect.anchoredPosition = new Vector2(10, -10);

                workAreasTitle = workAreasTitleObj.AddComponent<TextMeshProUGUI>();
                workAreasTitle.text = "Work Areas";
                workAreasTitle.alignment = TextAlignmentOptions.Left;
                workAreasTitle.fontSize = 36;
                workAreasTitle.color = Color.white;
            }
        }

        private void UpdateSettlementDetails()
        {
            if (currentSettlement == null) return;

            settlementTitle.text = currentSettlement.settlementName;
            settlementDetails.text = $"Founded: {currentSettlement.foundedDate}\n" +
                                   $"Population: {currentSettlement.Population}\n" +
                                   $"Work Areas: {currentSettlement.workAreas.Count}";
        }

        private void UpdateNPCsList()
        {
            if (currentSettlement == null) return;

            // Clear existing NPC texts
            foreach (var text in npcTexts)
            {
                Destroy(text.gameObject);
            }
            npcTexts.Clear();

            // Create new NPC texts
            float yOffset = -60f; // Start below the title
            foreach (var npc in currentSettlement.npcs)
            {
                GameObject npcObj = new GameObject($"NPC_{npc.npcName}");
                npcObj.transform.SetParent(npcsPanel.transform);
                
                RectTransform rectTransform = npcObj.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = new Vector2(380, 30);
                rectTransform.anchoredPosition = new Vector2(10, yOffset);

                TextMeshProUGUI npcText = npcObj.AddComponent<TextMeshProUGUI>();
                npcText.text = $"{npc.npcName} - {npc.occupation}";
                npcText.alignment = TextAlignmentOptions.Left;
                npcText.fontSize = 36;
                npcText.color = Color.white;

                npcTexts.Add(npcText);
                yOffset -= 40f; // Move down for next NPC
            }
        }

        private void UpdateWorkAreasList()
        {
            if (currentSettlement == null) return;

            // Clear existing work area texts
            foreach (var text in workAreaTexts)
            {
                Destroy(text.gameObject);
            }
            workAreaTexts.Clear();

            // Create new work area texts
            float yOffset = -80f; // Start below the title
            foreach (var workArea in currentSettlement.workAreas)
            {
                GameObject workAreaObj = new GameObject($"WorkArea_{workArea.areaName}");
                workAreaObj.transform.SetParent(workAreasPanel.transform);
                
                RectTransform rectTransform = workAreaObj.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = new Vector2(700, 30);
                rectTransform.anchoredPosition = new Vector2(150, yOffset);

                TextMeshProUGUI workAreaText = workAreaObj.AddComponent<TextMeshProUGUI>();
                workAreaText.text = $"{workArea.areaType} - {workArea.outputResource} ({workArea.outputAmount}/hr)";
                workAreaText.alignment = TextAlignmentOptions.Left;
                workAreaText.fontSize = 36;
                workAreaText.color = Color.white;

                workAreaTexts.Add(workAreaText);
                yOffset -= 40f; // Move down for next work area
            }
        }

        public void HandleMouseClick(Vector2 mousePosition)
        {
            // Handle clicks on detail page elements here
            // For now, this is a placeholder that can be expanded later
            Debug.Log("Click detected on Settlement Detail Page");
            
            // TODO: Implement click handling for various elements in the detail page
        }
    }
} 
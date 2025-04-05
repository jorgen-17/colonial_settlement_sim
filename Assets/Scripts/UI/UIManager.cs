using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using css.core;
using UnityEngine.Assertions;

namespace css.ui
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Main UI Panels")]
        public GameObject mainPanel;
        public GameObject settlementPanel;
        public GameObject npcPanel;
        public GameObject workAreaPanel;
        public GameObject resourcePanel;

        [Header("Settlement Info")]
        public TextMeshProUGUI settlementNameText;
        public TextMeshProUGUI populationText;
        public TextMeshProUGUI resourceCountText;

        [Header("NPC Info")]
        public GameObject npcListContent;
        public GameObject npcInfoPanel;
        public GameObject npcListItemPrefab;

        [Header("Work Area Info")]
        public GameObject workAreaListContent;
        public GameObject workAreaListItemPrefab;

        [Header("Resource Info")]
        public GameObject resourceListContent;
        public GameObject resourceListItemPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Assert.IsNotNull(GameManager.Instance, "GameManager not found in scene! Please add a GameObject with the GameManager component.");
            InitializeUI();
        }

        private void InitializeUI()
        {
            Assert.IsNotNull(GameManager.Instance, "GameManager.Instance is null during UI initialization");
            
            // Initialize all UI elements
            UpdateSettlementInfo();
            UpdateNPCList();
            UpdateWorkAreaList();
            UpdateResourceList();
        }

        public void UpdateSettlementInfo()
        {
            Assert.IsNotNull(GameManager.Instance, "GameManager.Instance is null during settlement info update");

            if (GameManager.Instance.settlements.Count > 0)
            {
                Settlement currentSettlement = GameManager.Instance.settlements[0]; // For now, just show the first settlement
                settlementNameText.text = currentSettlement.settlementName;
                populationText.text = $"Population: {currentSettlement.population}";

                // Update resource count
                string resourceText = "Resources:\n";
                foreach (var resource in currentSettlement.resources)
                {
                    resourceText += $"{resource.Key.name}: {resource.Value:F1}\n";
                }
                resourceCountText.text = resourceText;
            }
            else
            {
                Debug.LogWarning("No settlements found in GameManager");
                settlementNameText.text = "No Settlement";
                populationText.text = "Population: 0";
                resourceCountText.text = "Resources: None";
            }
        }

        public void UpdateNPCList()
        {
            Assert.IsNotNull(GameManager.Instance, "GameManager.Instance is null during NPC list update");
            
            // Clear existing NPC list items
            foreach (Transform child in npcListContent.transform)
            {
                Destroy(child.gameObject);
            }

            // Create new NPC list items
            foreach (NPC npc in GameManager.Instance.npcs)
            {
                GameObject npcItem = Instantiate(npcListItemPrefab, npcListContent.transform);
                npcItem.GetComponent<NPCListItem>().Initialize(npc);
            }
        }

        public void UpdateWorkAreaList()
        {
            Assert.IsNotNull(GameManager.Instance, "GameManager.Instance is null during work area list update");
            
            // Clear existing work area list items
            foreach (Transform child in workAreaListContent.transform)
            {
                Destroy(child.gameObject);
            }

            // Create new work area list items
            foreach (Settlement settlement in GameManager.Instance.settlements)
            {
                foreach (WorkArea workArea in settlement.workAreas)
                {
                    GameObject workAreaItem = Instantiate(workAreaListItemPrefab, workAreaListContent.transform);
                    workAreaItem.GetComponent<WorkAreaListItem>().Initialize(workArea);
                }
            }
        }

        public void UpdateResourceList()
        {
            Assert.IsNotNull(GameManager.Instance, "GameManager.Instance is null during resource list update");
            
            // Clear existing resource list items
            foreach (Transform child in resourceListContent.transform)
            {
                Destroy(child.gameObject);
            }

            // Create new resource list items
            foreach (ResourceType resource in GameManager.Instance.resourceTypes)
            {
                GameObject resourceItem = Instantiate(resourceListItemPrefab, resourceListContent.transform);
                resourceItem.GetComponent<ResourceListItem>().Initialize(resource);
            }
        }

        public void ShowNPCInfo(NPC npc)
        {
            npcInfoPanel.SetActive(true);
            npcInfoPanel.GetComponent<NPCInfoPanel>().ShowNPC(npc);
        }

        public void TogglePanel(GameObject panel)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
}
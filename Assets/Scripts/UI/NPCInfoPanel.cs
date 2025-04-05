using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class NPCInfoPanel : MonoBehaviour
    {
        [Header("Basic Info")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI occupationText;
        public TextMeshProUGUI stateText;

        [Header("Stats")]
        public Slider healthSlider;
        public Slider energySlider;
        public Slider hungerSlider;
        public Slider thirstSlider;

        [Header("Schedule")]
        public TextMeshProUGUI workHoursText;
        public TextMeshProUGUI sleepHoursText;

        [Header("Inventory")]
        public GameObject inventoryContent;
        public GameObject inventoryItemPrefab;

        [Header("Work Route")]
        public GameObject workRouteContent;
        public GameObject workRouteItemPrefab;

        private NPC currentNPC;

        public void ShowNPC(NPC npc)
        {
            currentNPC = npc;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (currentNPC == null) return;

            // Update basic info
            nameText.text = currentNPC.npcName;
            occupationText.text = currentNPC.occupation;
            stateText.text = currentNPC.currentState.ToString();

            // Update stats
            healthSlider.value = currentNPC.health / 100f;
            energySlider.value = currentNPC.energy / 100f;
            hungerSlider.value = currentNPC.hunger / 100f;
            thirstSlider.value = currentNPC.thirst / 100f;

            // Update schedule
            workHoursText.text = $"Work Hours: {currentNPC.workStartHour:00}:00 - {currentNPC.workEndHour:00}:00";
            sleepHoursText.text = $"Sleep Hours: {currentNPC.sleepStartHour:00}:00 - {currentNPC.sleepEndHour:00}:00";

            // Update inventory and work route
            UpdateInventory();
            UpdateWorkRoute();
        }

        private void UpdateInventory()
        {
            // Clear existing inventory items
            foreach (Transform child in inventoryContent.transform)
            {
                Destroy(child.gameObject);
            }

            // Create new inventory items
            foreach (var item in currentNPC.inventory)
            {
                if (item.Value > 0)
                {
                    GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryContent.transform);
                    inventoryItem.GetComponent<InventoryItem>().Initialize(item.Key, item.Value);
                }
            }
        }

        private void UpdateWorkRoute()
        {
            // Clear existing work route items
            foreach (Transform child in workRouteContent.transform)
            {
                Destroy(child.gameObject);
            }

            // Create new work route items
            for (int i = 0; i < currentNPC.workRoute.Count; i++)
            {
                GameObject workRouteItem = Instantiate(workRouteItemPrefab, workRouteContent.transform);
                workRouteItem.GetComponent<WorkRouteItem>().Initialize(currentNPC.workRoute[i], i, i == currentNPC.currentRouteIndex);
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
} 
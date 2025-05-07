using UnityEngine;
using TMPro;
using css.core;
using System.Collections.Generic;

namespace css.ui
{
    public class NPCDetailPage : MonoBehaviour, MenuPage
    {
        [Header("UI References")]
        public GameObject detailPanel;
        public GameObject pageGameObject;
        public TextMeshProUGUI npcTitle;
        
        // Top Left - Basic Info
        public GameObject basicInfoPanel;
        public TextMeshProUGUI basicInfoTitle;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI energyText;
        public TextMeshProUGUI hungerText;
        public TextMeshProUGUI thirstText;
        
        // Bottom Left - Inventory
        public GameObject inventoryPanel;
        public TextMeshProUGUI inventoryTitle;
        public List<TextMeshProUGUI> inventoryTexts = new List<TextMeshProUGUI>();
        
        // Top Right - Schedule & Current State
        public GameObject schedulePanel;
        public TextMeshProUGUI scheduleTitle;
        public TextMeshProUGUI workHoursText;
        public TextMeshProUGUI sleepHoursText;
        public TextMeshProUGUI currentStateText;
        
        // Bottom Right - Work Route
        public GameObject workRoutePanel;
        public TextMeshProUGUI workRouteTitle;
        public List<TextMeshProUGUI> workRouteTexts = new List<TextMeshProUGUI>();

        private Transform parentTransform;
        private NPC currentNPC;

        public void Initialize(Transform parent, GameObject pageGameObject)
        {
            parentTransform = parent;
            this.pageGameObject = pageGameObject;
            CreateDetailPanel();
        }

        public void SetNPC(NPC npc)
        {
            currentNPC = npc;
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

                // Create NPC title
                GameObject titleObj = new GameObject("NPCTitle");
                titleObj.transform.SetParent(detailPanel.transform);
                
                RectTransform titleRect = titleObj.AddComponent<RectTransform>();
                titleRect.anchorMin = new Vector2(0f, 1f);
                titleRect.anchorMax = new Vector2(0f, 1f);
                titleRect.sizeDelta = new Vector2(900, 60);
                titleRect.anchoredPosition = new Vector2(350, 0);

                npcTitle = titleObj.AddComponent<TextMeshProUGUI>();
                npcTitle.alignment = TextAlignmentOptions.Left;
                npcTitle.fontSize = 48;
                npcTitle.color = Color.white;
                
                // Create the four quadrant panels
                CreateBasicInfoPanel();
                CreateInventoryPanel();
                CreateSchedulePanel();
                CreateWorkRoutePanel();
            }
        }
        
        private void CreateBasicInfoPanel()
        {
            // Top Left - Basic Info
            basicInfoPanel = new GameObject("BasicInfoPanel");
            basicInfoPanel.transform.SetParent(detailPanel.transform);
            
            RectTransform basicInfoRect = basicInfoPanel.AddComponent<RectTransform>();
            basicInfoRect.anchorMin = new Vector2(0f, 0.5f);
            basicInfoRect.anchorMax = new Vector2(0.5f, 1f);
            basicInfoRect.sizeDelta = new Vector2(400, 350);
            basicInfoRect.anchoredPosition = new Vector2(-400, -250);
            
            // Create Basic Info title
            GameObject basicInfoTitleObj = new GameObject("BasicInfoTitle");
            basicInfoTitleObj.transform.SetParent(basicInfoPanel.transform);
            
            RectTransform basicInfoTitleRect = basicInfoTitleObj.AddComponent<RectTransform>();
            basicInfoTitleRect.anchorMin = new Vector2(0f, 1f);
            basicInfoTitleRect.anchorMax = new Vector2(1f, 1f);
            basicInfoTitleRect.sizeDelta = new Vector2(380, 40);
            basicInfoTitleRect.anchoredPosition = new Vector2(200, -20);
            
            basicInfoTitle = basicInfoTitleObj.AddComponent<TextMeshProUGUI>();
            basicInfoTitle.text = "Basic Information";
            basicInfoTitle.alignment = TextAlignmentOptions.Center;
            basicInfoTitle.fontSize = 36;
            basicInfoTitle.color = Color.white;
            
            // Create health text
            GameObject healthObj = new GameObject("HealthText");
            healthObj.transform.SetParent(basicInfoPanel.transform);
            
            RectTransform healthRect = healthObj.AddComponent<RectTransform>();
            healthRect.anchorMin = new Vector2(0f, 1f);
            healthRect.anchorMax = new Vector2(1f, 1f);
            healthRect.sizeDelta = new Vector2(380, 30);
            healthRect.anchoredPosition = new Vector2(200, -70);
            
            healthText = healthObj.AddComponent<TextMeshProUGUI>();
            healthText.alignment = TextAlignmentOptions.Center;
            healthText.fontSize = 28;
            healthText.color = Color.white;
            
            // Create energy text
            GameObject energyObj = new GameObject("EnergyText");
            energyObj.transform.SetParent(basicInfoPanel.transform);
            
            RectTransform energyRect = energyObj.AddComponent<RectTransform>();
            energyRect.anchorMin = new Vector2(0f, 1f);
            energyRect.anchorMax = new Vector2(1f, 1f);
            energyRect.sizeDelta = new Vector2(380, 30);
            energyRect.anchoredPosition = new Vector2(200, -120);
            
            energyText = energyObj.AddComponent<TextMeshProUGUI>();
            energyText.alignment = TextAlignmentOptions.Center;
            energyText.fontSize = 28;
            energyText.color = Color.white;
            
            // Create hunger text
            GameObject hungerObj = new GameObject("HungerText");
            hungerObj.transform.SetParent(basicInfoPanel.transform);
            
            RectTransform hungerRect = hungerObj.AddComponent<RectTransform>();
            hungerRect.anchorMin = new Vector2(0f, 1f);
            hungerRect.anchorMax = new Vector2(1f, 1f);
            hungerRect.sizeDelta = new Vector2(380, 30);
            hungerRect.anchoredPosition = new Vector2(200, -170);
            
            hungerText = hungerObj.AddComponent<TextMeshProUGUI>();
            hungerText.alignment = TextAlignmentOptions.Center;
            hungerText.fontSize = 28;
            hungerText.color = Color.white;
            
            // Create thirst text
            GameObject thirstObj = new GameObject("ThirstText");
            thirstObj.transform.SetParent(basicInfoPanel.transform);
            
            RectTransform thirstRect = thirstObj.AddComponent<RectTransform>();
            thirstRect.anchorMin = new Vector2(0f, 1f);
            thirstRect.anchorMax = new Vector2(1f, 1f);
            thirstRect.sizeDelta = new Vector2(380, 30);
            thirstRect.anchoredPosition = new Vector2(200, -220);
            
            thirstText = thirstObj.AddComponent<TextMeshProUGUI>();
            thirstText.alignment = TextAlignmentOptions.Center;
            thirstText.fontSize = 28;
            thirstText.color = Color.white;
        }
        
        private void CreateInventoryPanel()
        {
            // Bottom Left - Inventory
            inventoryPanel = new GameObject("InventoryPanel");
            inventoryPanel.transform.SetParent(detailPanel.transform);
            
            RectTransform inventoryRect = inventoryPanel.AddComponent<RectTransform>();
            inventoryRect.anchorMin = new Vector2(0f, 0f);
            inventoryRect.anchorMax = new Vector2(0.5f, 0.5f);
            inventoryRect.sizeDelta = new Vector2(400, 350);
            inventoryRect.anchoredPosition = new Vector2(-450, -200);
            
            // Create Inventory title
            GameObject inventoryTitleObj = new GameObject("InventoryTitle");
            inventoryTitleObj.transform.SetParent(inventoryPanel.transform);
            
            RectTransform inventoryTitleRect = inventoryTitleObj.AddComponent<RectTransform>();
            inventoryTitleRect.anchorMin = new Vector2(0f, 1f);
            inventoryTitleRect.anchorMax = new Vector2(1f, 1f);
            inventoryTitleRect.sizeDelta = new Vector2(380, 40);
            inventoryTitleRect.anchoredPosition = new Vector2(200, -20);
            
            inventoryTitle = inventoryTitleObj.AddComponent<TextMeshProUGUI>();
            inventoryTitle.text = "Inventory";
            inventoryTitle.alignment = TextAlignmentOptions.Center;
            inventoryTitle.fontSize = 36;
            inventoryTitle.color = Color.white;
        }
        
        private void CreateSchedulePanel()
        {
            // Top Right - Schedule & Current State
            schedulePanel = new GameObject("SchedulePanel");
            schedulePanel.transform.SetParent(detailPanel.transform);
            
            RectTransform scheduleRect = schedulePanel.AddComponent<RectTransform>();
            scheduleRect.anchorMin = new Vector2(0.5f, 0.5f);
            scheduleRect.anchorMax = new Vector2(1f, 1f);
            scheduleRect.sizeDelta = new Vector2(400, 350);
            scheduleRect.anchoredPosition = new Vector2(0, -250);
            
            // Create Schedule title
            GameObject scheduleTitleObj = new GameObject("ScheduleTitle");
            scheduleTitleObj.transform.SetParent(schedulePanel.transform);
            
            RectTransform scheduleTitleRect = scheduleTitleObj.AddComponent<RectTransform>();
            scheduleTitleRect.anchorMin = new Vector2(0f, 1f);
            scheduleTitleRect.anchorMax = new Vector2(1f, 1f);
            scheduleTitleRect.sizeDelta = new Vector2(380, 40);
            scheduleTitleRect.anchoredPosition = new Vector2(200, -20);
            
            scheduleTitle = scheduleTitleObj.AddComponent<TextMeshProUGUI>();
            scheduleTitle.text = "Schedule & Current State";
            scheduleTitle.alignment = TextAlignmentOptions.Center;
            scheduleTitle.fontSize = 36;
            scheduleTitle.color = Color.white;
            
            // Create work hours text
            GameObject workHoursObj = new GameObject("WorkHoursText");
            workHoursObj.transform.SetParent(schedulePanel.transform);
            
            RectTransform workHoursRect = workHoursObj.AddComponent<RectTransform>();
            workHoursRect.anchorMin = new Vector2(0f, 1f);
            workHoursRect.anchorMax = new Vector2(1f, 1f);
            workHoursRect.sizeDelta = new Vector2(380, 30);
            workHoursRect.anchoredPosition = new Vector2(200, -70);
            
            workHoursText = workHoursObj.AddComponent<TextMeshProUGUI>();
            workHoursText.alignment = TextAlignmentOptions.Center;
            workHoursText.fontSize = 28;
            workHoursText.color = Color.white;
            
            // Create sleep hours text
            GameObject sleepHoursObj = new GameObject("SleepHoursText");
            sleepHoursObj.transform.SetParent(schedulePanel.transform);
            
            RectTransform sleepHoursRect = sleepHoursObj.AddComponent<RectTransform>();
            sleepHoursRect.anchorMin = new Vector2(0f, 1f);
            sleepHoursRect.anchorMax = new Vector2(1f, 1f);
            sleepHoursRect.sizeDelta = new Vector2(380, 30);
            sleepHoursRect.anchoredPosition = new Vector2(200, -120);
            
            sleepHoursText = sleepHoursObj.AddComponent<TextMeshProUGUI>();
            sleepHoursText.alignment = TextAlignmentOptions.Center;
            sleepHoursText.fontSize = 28;
            sleepHoursText.color = Color.white;
            
            // Create current state text
            GameObject currentStateObj = new GameObject("CurrentStateText");
            currentStateObj.transform.SetParent(schedulePanel.transform);
            
            RectTransform currentStateRect = currentStateObj.AddComponent<RectTransform>();
            currentStateRect.anchorMin = new Vector2(0f, 1f);
            currentStateRect.anchorMax = new Vector2(1f, 1f);
            currentStateRect.sizeDelta = new Vector2(380, 30);
            currentStateRect.anchoredPosition = new Vector2(200, -170);
            
            currentStateText = currentStateObj.AddComponent<TextMeshProUGUI>();
            currentStateText.alignment = TextAlignmentOptions.Center;
            currentStateText.fontSize = 28;
            currentStateText.color = Color.white;
        }
        
        private void CreateWorkRoutePanel()
        {
            // Bottom Right - Work Route
            workRoutePanel = new GameObject("WorkRoutePanel");
            workRoutePanel.transform.SetParent(detailPanel.transform);
            
            RectTransform workRouteRect = workRoutePanel.AddComponent<RectTransform>();
            workRouteRect.anchorMin = new Vector2(0.5f, 0f);
            workRouteRect.anchorMax = new Vector2(1f, 0.5f);
            workRouteRect.sizeDelta = new Vector2(400, 350);
            workRouteRect.anchoredPosition = new Vector2(0, -200);
            
            // Create Work Route title
            GameObject workRouteTitleObj = new GameObject("WorkRouteTitle");
            workRouteTitleObj.transform.SetParent(workRoutePanel.transform);
            
            RectTransform workRouteTitleRect = workRouteTitleObj.AddComponent<RectTransform>();
            workRouteTitleRect.anchorMin = new Vector2(0f, 1f);
            workRouteTitleRect.anchorMax = new Vector2(1f, 1f);
            workRouteTitleRect.sizeDelta = new Vector2(380, 40);
            workRouteTitleRect.anchoredPosition = new Vector2(200, -20);
            
            workRouteTitle = workRouteTitleObj.AddComponent<TextMeshProUGUI>();
            workRouteTitle.text = "Work Route";
            workRouteTitle.alignment = TextAlignmentOptions.Center;
            workRouteTitle.fontSize = 36;
            workRouteTitle.color = Color.white;
        }

        private void UpdateNPCDetails()
        {
            if (currentNPC == null) return;

            // Update title
            npcTitle.text = $"{currentNPC.npcName} - {currentNPC.occupation}";
            
            // Update basic info section
            healthText.text = $"Health: {currentNPC.health:F1}";
            energyText.text = $"Energy: {currentNPC.energy:F1}";
            hungerText.text = $"Hunger: {currentNPC.hunger:F1}";
            thirstText.text = $"Thirst: {currentNPC.thirst:F1}";
            
            // Update schedule section
            workHoursText.text = $"Work Hours: {currentNPC.workStartHour:F1} - {currentNPC.workEndHour:F1}";
            sleepHoursText.text = $"Sleep Hours: {currentNPC.sleepStartHour:F1} - {currentNPC.sleepEndHour:F1}";
            currentStateText.text = $"Current State: {currentNPC.currentState}";

            UpdateInventoryList();
            UpdateWorkRouteList();
        }

        private void UpdateInventoryList()
        {
            if (currentNPC == null) return;

            // Clear existing inventory texts
            foreach (var text in inventoryTexts)
            {
                Destroy(text.gameObject);
            }
            inventoryTexts.Clear();

            // Create new inventory texts
            float yOffset = -60f; // Start below the title
            
            // Add money display first
            GameObject moneyObj = new GameObject("MoneyText");
            moneyObj.transform.SetParent(inventoryPanel.transform);
            
            RectTransform moneyRect = moneyObj.AddComponent<RectTransform>();
            moneyRect.anchorMin = new Vector2(0f, 1f);
            moneyRect.anchorMax = new Vector2(1f, 1f);
            moneyRect.sizeDelta = new Vector2(380, 30);
            moneyRect.anchoredPosition = new Vector2(200, yOffset);
            
            TextMeshProUGUI moneyText = moneyObj.AddComponent<TextMeshProUGUI>();
            moneyText.text = $"Money: ${currentNPC.money:F2}";
            moneyText.alignment = TextAlignmentOptions.Center;
            moneyText.fontSize = 28;
            moneyText.color = Color.white;
            
            inventoryTexts.Add(moneyText);
            yOffset -= 40f;
            
            // Add all resources in inventory
            foreach (var item in currentNPC.inventory)
            {
                if (item.baseValue > 0)
                {
                    // todo add ids to resources
                    GameObject resourceObj = new GameObject($"Resource_{item.type}");
                    resourceObj.transform.SetParent(inventoryPanel.transform);
                    
                    RectTransform resourceRect = resourceObj.AddComponent<RectTransform>();
                    resourceRect.anchorMin = new Vector2(0f, 1f);
                    resourceRect.anchorMax = new Vector2(1f, 1f);
                    resourceRect.sizeDelta = new Vector2(380, 30);
                    resourceRect.anchoredPosition = new Vector2(200, yOffset);
                    
                    TextMeshProUGUI resourceText = resourceObj.AddComponent<TextMeshProUGUI>();
                    resourceText.text = $"{item.type}: {item.amount:F1}";
                    resourceText.alignment = TextAlignmentOptions.Center;
                    resourceText.fontSize = 28;
                    resourceText.color = Color.white;
                    
                    inventoryTexts.Add(resourceText);
                    yOffset -= 40f;
                }
            }
        }

        private void UpdateWorkRouteList()
        {
            if (currentNPC == null) return;

            // Clear existing work route texts
            foreach (var text in workRouteTexts)
            {
                Destroy(text.gameObject);
            }
            workRouteTexts.Clear();

            // Create new work route texts
            float yOffset = -60f; // Start below the title
            
            // Add current work area info
            if (currentNPC.currentWorkArea != null)
            {
                GameObject currentAreaObj = new GameObject("CurrentAreaText");
                currentAreaObj.transform.SetParent(workRoutePanel.transform);
                
                RectTransform currentAreaRect = currentAreaObj.AddComponent<RectTransform>();
                currentAreaRect.anchorMin = new Vector2(0f, 1f);
                currentAreaRect.anchorMax = new Vector2(1f, 1f);
                currentAreaRect.sizeDelta = new Vector2(380, 30);
                currentAreaRect.anchoredPosition = new Vector2(200, yOffset);
                
                TextMeshProUGUI currentAreaText = currentAreaObj.AddComponent<TextMeshProUGUI>();
                currentAreaText.text = $"Current: {currentNPC.currentWorkArea.areaType.ToString()}";
                currentAreaText.alignment = TextAlignmentOptions.Center;
                currentAreaText.fontSize = 28;
                currentAreaText.color = Color.yellow;
                
                workRouteTexts.Add(currentAreaText);
                yOffset -= 40f;
                
                // Show progress at current area
                GameObject progressObj = new GameObject("ProgressText");
                progressObj.transform.SetParent(workRoutePanel.transform);
                
                RectTransform progressRect = progressObj.AddComponent<RectTransform>();
                progressRect.anchorMin = new Vector2(0f, 1f);
                progressRect.anchorMax = new Vector2(1f, 1f);
                progressRect.sizeDelta = new Vector2(380, 30);
                progressRect.anchoredPosition = new Vector2(200, yOffset);
                
                TextMeshProUGUI progressText = progressObj.AddComponent<TextMeshProUGUI>();
                progressText.text = $"Progress: {currentNPC.GetTimeWorkedAtCurrentArea():F1}/{currentNPC.requiredTimeAtCurrentArea:F1}";
                progressText.alignment = TextAlignmentOptions.Center;
                progressText.fontSize = 28;
                progressText.color = Color.white;
                
                workRouteTexts.Add(progressText);
                yOffset -= 40f;
            }
            
            // Add separator
            GameObject separatorObj = new GameObject("SeparatorText");
            separatorObj.transform.SetParent(workRoutePanel.transform);
            
            RectTransform separatorRect = separatorObj.AddComponent<RectTransform>();
            separatorRect.anchorMin = new Vector2(0f, 1f);
            separatorRect.anchorMax = new Vector2(1f, 1f);
            separatorRect.sizeDelta = new Vector2(380, 30);
            separatorRect.anchoredPosition = new Vector2(200, yOffset);
            
            TextMeshProUGUI separatorText = separatorObj.AddComponent<TextMeshProUGUI>();
            separatorText.text = "Route:";
            separatorText.alignment = TextAlignmentOptions.Center;
            separatorText.fontSize = 28;
            separatorText.color = Color.white;
            
            workRouteTexts.Add(separatorText);
            yOffset -= 40f;
            
            // Show full work route
            int index = 0;
            foreach (var workArea in currentNPC.workRoute)
            {
                GameObject areaObj = new GameObject($"Area_{workArea.id}");
                areaObj.transform.SetParent(workRoutePanel.transform);
                
                RectTransform areaRect = areaObj.AddComponent<RectTransform>();
                areaRect.anchorMin = new Vector2(0f, 1f);
                areaRect.anchorMax = new Vector2(1f, 1f);
                areaRect.sizeDelta = new Vector2(380, 30);
                areaRect.anchoredPosition = new Vector2(200, yOffset);
                
                TextMeshProUGUI areaText = areaObj.AddComponent<TextMeshProUGUI>();
                string areaPrefix = (index == currentNPC.currentRouteIndex) ? "→ " : "   ";
                areaText.text = $"{areaPrefix}{index + 1}. {workArea.areaType}";
                areaText.alignment = TextAlignmentOptions.Center;
                areaText.fontSize = 28;
                areaText.color = (index == currentNPC.currentRouteIndex) ? Color.green : Color.white;
                
                workRouteTexts.Add(areaText);
                yOffset -= 40f;
                index++;
            }
        }

        public void HandleMouseClick(Vector2 mousePosition)
        {
            // No specific mouse handling needed for now
        }

        public void Show()
        {
            pageGameObject.SetActive(true);
            detailPanel.SetActive(true); 
        }
        
        public void Hide()
        {
            pageGameObject.SetActive(false);
            detailPanel.SetActive(false); 
        }
        
        public bool IsActive()
        {
            return pageGameObject.activeInHierarchy && detailPanel.activeInHierarchy;
        }

        public void Update()
        {
            UpdateNPCDetails();
        }
    }
} 
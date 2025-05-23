using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;
using System;
using System.Collections.Generic;

namespace css.ui
{
    public class MenuUI : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject menuPanel;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI dayText;
        public SettlementsListPage settlementsPage;
        public SettlementsDetailPage settlementsDetailPage;
        public MenuPage npcDetailPage;
        
        // Back button
        private Button backButton;
        private TextMeshProUGUI backButtonText;
        
        // Page tracking
        private MenuPage activePage;
        private Dictionary<string, MenuPage> menuPagesMap = new Dictionary<string, MenuPage>();
        
        // Event history tracking
        private Stack<UIEventRecord> backStack = new Stack<UIEventRecord>();
        private UIEventRecord lastEvent;

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

            // Create back button
            CreateBackButton();

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

            // Create settlements page if it doesn't exist
            if (settlementsPage == null)
            {
                GameObject settlementsPageObj = new GameObject("SettlementsPage");
                settlementsPageObj.transform.SetParent(menuPanel.transform);
                settlementsPage = settlementsPageObj.AddComponent<SettlementsListPage>();
                settlementsPage.Initialize(menuPanel.transform, settlementsPageObj);
                settlementsPage.Show();
                UpdateEventHistory(new PageChangeEvent(nameof(SettlementsListPage)));
            }

            // Create settlements detail page if it doesn't exist
            if (settlementsDetailPage == null)
            {
                GameObject settlementsDetailPageObj = new GameObject("SettlementsDetailPage");
                settlementsDetailPageObj.transform.SetParent(menuPanel.transform);
                settlementsDetailPage = settlementsDetailPageObj.AddComponent<SettlementsDetailPage>();
                settlementsDetailPage.Initialize(menuPanel.transform, settlementsDetailPageObj);
                settlementsDetailPage.Hide();
            }
            
            // Create NPC detail page if it doesn't exist
            if (npcDetailPage == null)
            {
                GameObject npcDetailPageObj = new GameObject("NPCDetailPage");
                npcDetailPageObj.transform.SetParent(menuPanel.transform);
                // Add the NPCDetailPage component using AddComponent<>() with string-based type loading
                MonoBehaviour pageComponent = (MonoBehaviour)npcDetailPageObj.AddComponent(Type.GetType("css.ui.NPCDetailPage, Assembly-CSharp"));
                npcDetailPage = (MenuPage)pageComponent;
                npcDetailPage.Initialize(menuPanel.transform, npcDetailPageObj);
                npcDetailPage.Hide();
            }
            
            // Initialize page mapping
            InitializePageMap();
            
            // Set default active page
            activePage = settlementsPage;
        }
        
        private void CreateBackButton()
        {
            // Create back button if it doesn't exist
            GameObject backButtonObj = new GameObject("BackButton");
            backButtonObj.transform.SetParent(menuPanel.transform);
            
            // Add RectTransform
            RectTransform backButtonRect = backButtonObj.AddComponent<RectTransform>();
            backButtonRect.anchorMin = new Vector2(0.5f, 0.5f);
            backButtonRect.anchorMax = new Vector2(0.5f, 0.5f);
            backButtonRect.sizeDelta = new Vector2(100, 40);
            backButtonRect.anchoredPosition = new Vector2(-880, 500);
            
            // Add Image component for background
            Image backButtonImage = backButtonObj.AddComponent<Image>();
            backButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            
            // Add Button component
            backButton = backButtonObj.AddComponent<Button>();
            backButton.targetGraphic = backButtonImage;
            ColorBlock colors = backButton.colors;
            colors.normalColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            colors.highlightedColor = new Color(0.4f, 0.4f, 0.4f, 1f);
            colors.pressedColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            backButton.colors = colors;
            
            // Add text to button
            GameObject backButtonTextObj = new GameObject("Text");
            backButtonTextObj.transform.SetParent(backButtonObj.transform);
            
            RectTransform textRect = backButtonTextObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            
            backButtonText = backButtonTextObj.AddComponent<TextMeshProUGUI>();
            backButtonText.text = "Back";
            backButtonText.color = Color.white;
            backButtonText.fontSize = 18;
            backButtonText.alignment = TextAlignmentOptions.Center;
            
            // Add click listener
            // onClick listener is not working, so we need to check if mouse click inside back button rect instead
            // backButton.onClick.AddListener(HandleBackButtonClick);
            
            // Initially hide the button
            backButton.gameObject.SetActive(false);
        }
        
        private void HandleBackButtonClick()
        {
            Debug.Log("Back button clicked");

            if (backStack.Count > 0)
            {
                // Pop the previous event from the stack
                UIEventRecord previousEvent = backStack.Pop();

                // avoid circular back button loop
                lastEvent = null;
                
                // Execute the appropriate event based on type
                switch (previousEvent)
                {
                    case PageChangeEvent pageChangeEvent:
                        UIEvents.RequestPageChange(pageChangeEvent.PageName);
                        break;
                    
                    case SettlementDetailEvent settlementDetailEvent:
                        UIEvents.RequestSettlementDetail(settlementDetailEvent.SettlementId);
                        break;
                        
                    case NPCDetailEvent npcDetailEvent:
                        UIEvents.RequestNPCDetail(npcDetailEvent.NPCId);
                        break;
                }
                
                // Update the button visibility
                UpdateBackButtonVisibility();
            }
        }
       
        private void UpdateBackButtonVisibility()
        {
            // Only show the back button if we have items in the stack
            backButton.gameObject.SetActive(backStack.Count > 0);
        }

        private void InitializePageMap()
        {
            // Add all pages to the map with their type names as keys
            menuPagesMap.Add(nameof(SettlementsListPage), settlementsPage);
            menuPagesMap.Add(nameof(SettlementsDetailPage), settlementsDetailPage);
            menuPagesMap.Add(nameof(NPCDetailPage), npcDetailPage);
            
            // You can add more pages here as they're created
        }

        private void OnEnable()
        {
            // Subscribe to UI events
            UIEvents.OnPageChangeRequested += HandlePageChangeRequest;
            UIEvents.OnSettlementDetailRequested += HandleSettlementDetailRequest;
            UIEvents.OnNPCDetailRequested += HandleNPCDetailRequest;
        }

        private void OnDisable()
        {
            // Unsubscribe from UI events
            UIEvents.OnPageChangeRequested -= HandlePageChangeRequest;
            UIEvents.OnSettlementDetailRequested -= HandleSettlementDetailRequest;
            UIEvents.OnNPCDetailRequested -= HandleNPCDetailRequest;
        }

        private void HandleSettlementDetailRequest(Guid settlementId)
        {
            Debug.Log($"MenuUI received request to show settlement details for ID: {settlementId}");

            // Find the settlement by ID
            Settlement settlement = GameManager.Instance.settlements.Find(s => s.id == settlementId);
            
            if (settlement != null)
            {
                // Disable all pages
                foreach (var page in menuPagesMap.Values)
                {
                    page.Hide();
                }

                // Get and enable the settlement detail page
                MenuPage detailPage = menuPagesMap[nameof(SettlementsDetailPage)];
                if (detailPage != null && detailPage is SettlementsDetailPage settlementsDetailPage)
                {
                    settlementsDetailPage.Show();
                
                    // Set it as the active page
                    activePage = settlementsDetailPage;
                
                    // Set the settlement data
                    settlementsDetailPage.SetSettlement(settlement);

                    UpdateEventHistory(new SettlementDetailEvent(settlementId));
           
                    Debug.Log($"Switched to Settlement Detail page for: {settlement.settlementName}");
                } 
                else
                {
                    Debug.LogWarning($"Could not find settlement detail page");
                }
            }
            else
            {
                Debug.LogWarning($"Could not find settlement with ID: {settlementId}");
            }
        }

        private void HandlePageChangeRequest(string pageName)
        {
            Debug.Log($"MenuUI received request to go to page: {pageName}");
            
            // Look for the requested page in our map
            if (menuPagesMap.TryGetValue(pageName, out MenuPage requestedPage))
            {
                // Disable all pages
                foreach (var page in menuPagesMap.Values)
                {
                    page.Hide();
                }
                
                // Enable the requested page
                requestedPage.Show();
                
                // Set it as the active page
                activePage = requestedPage;
                
                UpdateEventHistory(new PageChangeEvent(pageName));
           
                Debug.Log($"Switched to page: {pageName}");
            }
            else
            {
                Debug.LogWarning($"Unknown page name: {pageName}");
            }
        }

        private void HandleNPCDetailRequest(Guid npcId)
        {
            Debug.Log($"MenuUI received request to show NPC details for ID: {npcId}");
            
            // Find the NPC by ID - we need to search all settlements
            NPC npc = null;
            foreach (var settlement in GameManager.Instance.settlements)
            {
                npc = settlement.npcs.Find(n => n.id == npcId);
                if (npc != null) break;
            }
            
            if (npc != null)
            {
                // Disable all pages
                foreach (var page in menuPagesMap.Values)
                {
                    page.Hide();
                }
                
                MenuPage detailPage = menuPagesMap[nameof(NPCDetailPage)];
                if (detailPage  != null && detailPage is NPCDetailPage npcDetailPage)
                {
                    // Show the page
                    npcDetailPage.Show();
                    
                    // Set it as the active page
                    activePage = npcDetailPage;
                    
                    npcDetailPage.SetNPC(npc);

                    UpdateEventHistory(new NPCDetailEvent(npcId));
                    
                    Debug.Log($"Switched to NPC Detail page for: {npc.npcName}");
                }
                else
                {
                    Debug.LogWarning($"Could not find NPC detail page");
                }
            }
            else
            {
                Debug.LogWarning($"Could not find NPC with ID: {npcId}");
            }
        }

        private void Update()
        {
            // Handle mouse clicks
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseClick();
            }
            
            // Only update the active page
            if (activePage != null && activePage.IsActive())
            {
                // For SettlementsListPage, we need to call its Update method explicitly
                // since it has its own Update implementation
                activePage.Update();
            }
        }

        private void HandleMouseClick()
        {
             // Get the current mouse position
            Vector2 mousePosition = Input.mousePosition;
 
            RectTransform rectTransform = backButton.GetComponent<RectTransform>();
            // Convert mouse position to local space of the settlement button
            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, mousePosition, null, out localMousePos);
                
            // Check if the mouse is inside the settlement button
            if (rectTransform.rect.Contains(localMousePos))
            {
                Debug.Log("Click detected on Back Button");
                HandleBackButtonClick();
            }
            
            // Only pass clicks to the active page if not hitting UI
            if (activePage != null && activePage.IsActive())
            {
                activePage.HandleMouseClick(mousePosition);
            }
        }

        private void UpdateEventHistory(UIEventRecord newLastEvent)
        {
            if (lastEvent != null)
            {
                backStack.Push(lastEvent);
            }
            
            lastEvent = newLastEvent;
            
            // Update back button visibility after history changes
            UpdateBackButtonVisibility();
        }
    }
} 
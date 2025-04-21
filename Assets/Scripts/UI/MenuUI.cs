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
        
        // Page tracking
        private MenuPage activePage;
        private Dictionary<string, MenuPage> menuPagesMap = new Dictionary<string, MenuPage>();

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

            // Create settlements page if it doesn't exist
            if (settlementsPage == null)
            {
                GameObject settlementsPageObj = new GameObject("SettlementsPage");
                settlementsPageObj.transform.SetParent(menuPanel.transform);
                settlementsPage = settlementsPageObj.AddComponent<SettlementsListPage>();
                settlementsPage.Initialize(menuPanel.transform, settlementsPageObj);
                settlementsPage.Show();
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
            
            // Initialize page mapping
            InitializePageMap();
            
            // Set default active page
            activePage = settlementsPage;
        }
        
        private void InitializePageMap()
        {
            // Add all pages to the map with their type names as keys
            menuPagesMap.Add(nameof(SettlementsListPage), settlementsPage);
            menuPagesMap.Add(nameof(SettlementsDetailPage), settlementsDetailPage);
            
            // You can add more pages here as they're created
        }

        private void OnEnable()
        {
            // Subscribe to UI events
            UIEvents.OnPageChangeRequested += HandlePageChangeRequest;
            UIEvents.OnSettlementDetailRequested += HandleSettlementDetailRequest;
        }

        private void OnDisable()
        {
            // Unsubscribe from UI events
            UIEvents.OnPageChangeRequested -= HandlePageChangeRequest;
            UIEvents.OnSettlementDetailRequested -= HandleSettlementDetailRequest;
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
                
                Debug.Log($"Switched to page: {pageName}");
            }
            else
            {
                Debug.LogWarning($"Unknown page name: {pageName}");
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
            
            // Only pass clicks to the active page
            if (activePage != null && activePage.IsActive())
            {
                activePage.HandleMouseClick(mousePosition);
            }
        }
    }
} 
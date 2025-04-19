using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;
using System;

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
                settlementsPage.Initialize(menuPanel.transform);
                settlementsPage.gameObject.SetActive(true); // Start visible
            }

            // Create settlements detail page if it doesn't exist
            if (settlementsDetailPage == null)
            {
                GameObject settlementsDetailPageObj = new GameObject("SettlementsDetailPage");
                settlementsDetailPageObj.transform.SetParent(menuPanel.transform);
                settlementsDetailPage = settlementsDetailPageObj.AddComponent<SettlementsDetailPage>();
                settlementsDetailPage.Initialize(menuPanel.transform);
                settlementsDetailPage.gameObject.SetActive(false); // Start hidden
            }
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
                // ShowSettlementDetails(settlement);
                Debug.Log($"Settlement clicked: {settlement.settlementName}");
            }
            else
            {
                Debug.LogWarning($"Could not find settlement with ID: {settlementId}");
            }
        }

        private void HandlePageChangeRequest(string pageName)
        {
            Debug.Log($"MenuUI received request to go to page: {pageName}");
            
            // Handle generic page navigation
            switch (pageName)
            {
                case "SettlementsList":
                    ShowSettlementsList();
                    break;
                default:
                    Debug.LogWarning($"Unknown page name: {pageName}");
                    break;
            }
        }

        private void Update()
        {
            // Handle mouse clicks
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseClick();
            }
            
            if (settlementsPage != null)
            {
                settlementsPage.Update();
            }
            // TODO: Uncomment this when we have a way to select a settlement
            // if (settlementsDetailPage != null && GameManager.Instance.settlements.Count > 0) {
            //     settlementsDetailPage.SetSettlement(GameManager.Instance.settlements[0]);
            // }
        }
        
        private void HandleMouseClick()
        {
            // Get the current mouse position
            Vector2 mousePosition = Input.mousePosition;
            
            // Check if settlements page is active and handle click
            if (settlementsPage != null && settlementsPage.gameObject.activeInHierarchy)
            {
                settlementsPage.HandleMouseClick(mousePosition);
            }
        }

        public void ShowSettlementDetails(Settlement settlement)
        {
            settlementsPage.gameObject.SetActive(false);
            settlementsDetailPage.gameObject.SetActive(true);
            settlementsDetailPage.SetSettlement(settlement);
        }

        public void ShowSettlementsList()
        {
            settlementsPage.gameObject.SetActive(true);
            settlementsDetailPage.gameObject.SetActive(false);
        }
    }
} 
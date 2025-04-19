using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

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

        private void Update()
        {
            if (settlementsPage != null)
            {
                settlementsPage.Update();
            }
            // TODO: Uncomment this when we have a way to select a settlement
            // if (settlementsDetailPage != null && GameManager.Instance.settlements.Count > 0) {
            //     settlementsDetailPage.SetSettlement(GameManager.Instance.settlements[0]);
            // }
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
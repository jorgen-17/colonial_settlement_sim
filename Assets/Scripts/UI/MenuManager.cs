using UnityEngine;
using TMPro;
using css.core;

namespace css.ui
{
    public class MenuManager : MonoBehaviour
    {
        [Header("UI References")]
        public MenuUI menuUI;

        private bool isMenuVisible = false;
        private PlayerController playerController;

        private void Awake()
        {
            // Find the PlayerController
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("PlayerController not found in scene!");
            }

            // Create MenuUI if it doesn't exist
            if (menuUI == null)
            {
                GameObject menuUIObj = new GameObject("MenuUI");
                menuUIObj.transform.SetParent(transform);
                menuUI = menuUIObj.AddComponent<MenuUI>();
            }

            // Initially hide the menu
            menuUI.menuPanel.SetActive(false);
        }

        private void Update()
        {
            // Toggle menu visibility when M key is pressed
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleMenu();
            }

            // Update time display if menu is visible
            if (isMenuVisible)
            {
                UpdateTimeDisplay();
            }
        }

        private void ToggleMenu()
        {
            isMenuVisible = !isMenuVisible;
            menuUI.menuPanel.SetActive(isMenuVisible);
            
            // Enable/disable camera control based on menu state
            if (playerController != null)
            {
                playerController.SetCameraEnabled(!isMenuVisible);
            }
        }

        private void UpdateTimeDisplay()
        {
            // Format time as HH:MM
            string timeString = $"{GameManager.Instance.CurrentHour:D2}:{GameManager.Instance.CurrentMinute:D2}";
            menuUI.timeText.text = $"Time: {timeString}";
            
            // Update day text
            menuUI.dayText.text = $"Day: {GameManager.Instance.currentDay}";
        }
    }
} 
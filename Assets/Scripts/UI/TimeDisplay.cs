using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class TimeDisplay : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI dayText;

        private void Update()
        {
            if (timeText != null)
            {
                // Format time as HH:MM
                timeText.text = $"{GameManager.Instance.CurrentHour:D2}:{GameManager.Instance.CurrentMinute:D2}";
            }

            if (dayText != null)
            {
                dayText.text = $"Day {GameManager.Instance.currentDay}";
            }
        }
    }
} 
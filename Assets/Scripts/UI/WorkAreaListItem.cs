using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class WorkAreaListItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI typeText;
        public TextMeshProUGUI workersText;
        public TextMeshProUGUI efficiencyText;
        public TextMeshProUGUI operationalText;
        public TextMeshProUGUI indoorText;
        
        private WorkArea workArea;
        
        public void Initialize(WorkArea workArea)
        {
            this.workArea = workArea;
            UpdateUI();
            
            // Add click listener to the button
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        
        private void UpdateUI()
        {
            if (workArea == null) return;
            
            nameText.text = workArea.areaName;
            typeText.text = workArea.areaType.ToString();
            workersText.text = $"Workers: {workArea.assignedWorkers.Count}/{workArea.maxWorkers}";
            efficiencyText.text = $"Efficiency: {workArea.GetEfficiency():P0}";
            operationalText.text = workArea.isOperational ? "Operational" : "Not Operational";
            indoorText.text = workArea.isIndoor ? "Indoor" : "Outdoor";
        }
        
        private void OnClick()
        {
            // UIManager.Instance.ShowWorkAreaInfo(workArea);
        }
    }
} 
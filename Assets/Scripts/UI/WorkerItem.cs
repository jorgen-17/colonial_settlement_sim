using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class WorkerItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI occupationText;
        public Slider efficiencySlider;
        
        public void Initialize(NPC worker)
        {
            nameText.text = worker.npcName;
            occupationText.text = worker.occupation;
            efficiencySlider.value = worker.energy / 100f; // Use energy as a rough efficiency indicator
        }
    }
} 
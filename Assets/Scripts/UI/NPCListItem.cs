using UnityEngine;
using UnityEngine.UI;
using TMPro;
using css.core;

namespace css.ui
{
    public class NPCListItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI occupationText;
        public TextMeshProUGUI stateText;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI energyText;
        public TextMeshProUGUI hungerText;
        public TextMeshProUGUI thirstText;
        
        private NPC npc;
        
        public void Initialize(NPC npc)
        {
            this.npc = npc;
            UpdateUI();
            
            // Add click listener to the button
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        
        private void UpdateUI()
        {
            if (npc == null) return;
            
            nameText.text = npc.npcName;
            occupationText.text = npc.occupation;
            stateText.text = npc.currentState.ToString();
            healthText.text = $"Health: {npc.health:F1}";
            energyText.text = $"Energy: {npc.energy:F1}";
            hungerText.text = $"Hunger: {npc.hunger:F1}";
            thirstText.text = $"Thirst: {npc.thirst:F1}";
        }
        
        private void OnClick()
        {
            UIManager.Instance.ShowNPCInfo(npc);
        }
    }
} 
using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Game Settings")]
        public float gameTimeScale = 1f;
        public float dayLength = 1200f; // 20 minutes per day at normal time scale
        
        [Header("Resource Types")]
        public List<ResourceType> resourceTypes = new List<ResourceType>();
        
        [Header("Settlements")]
        public List<Settlement> settlements = new List<Settlement>();
        
        [Header("NPCs")]
        public List<NPC> npcs = new List<NPC>();
        
        private float currentGameTime = 0f;
        private int currentDay = 0;
        
        public float CurrentGameTime => currentGameTime;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            InitializeGame();
        }
        
        private void Update()
        {
            UpdateGameTime();
        }
        
        private void InitializeGame()
        {
            // Initialize resource types
            InitializeResourceTypes();
            
            // Initialize settlements
            InitializeSettlements();
            
            // Initialize NPCs and their jobs
            InitializeNPCs();
        }
        
        private void InitializeResourceTypes()
        {
            // Initialize basic resources
            resourceTypes.Add(new ResourceType("Wood", "Raw material from trees"));
            resourceTypes.Add(new ResourceType("Meat", "Raw meat from animals"));
            resourceTypes.Add(new ResourceType("Fish", "Raw fish from water bodies"));
            resourceTypes.Add(new ResourceType("Stone", "Raw stone from quarries"));
            resourceTypes.Add(new ResourceType("Iron Ore", "Raw iron ore from mines"));
            resourceTypes.Add(new ResourceType("Coal", "Raw coal from mines"));
        }
        
        private void InitializeSettlements()
        {
            // Create a default settlement
            GameObject settlementObj = new GameObject("Default Settlement");
            Settlement settlement = settlementObj.AddComponent<Settlement>();
            settlement.settlementName = "New Town";
            settlement.population = 10; // Start with 10 people
            settlement.money = 1000f; // Start with some money
            settlements.Add(settlement);
        }
        
        private void InitializeNPCs()
        {
            // Find all NPCs in the scene and add them to the list
            NPC[] sceneNPCs = Object.FindObjectsByType<NPC>(FindObjectsSortMode.None);
            npcs.AddRange(sceneNPCs);
            // todo create a json file that defines settlements and their npcs then
            // load the json file and populate the settlements and npcs lists
            
            foreach (NPC npc in npcs)
            {
                // NPC initialization is handled in the NPC class's Start method
            }
        }
        
        private void UpdateGameTime()
        {
            currentGameTime += Time.deltaTime * gameTimeScale;
            
            if (currentGameTime >= dayLength)
            {
                currentGameTime = 0f;
                currentDay++;
                OnNewDay();
            }
        }
        
        private void OnNewDay()
        {
            // Update all settlements' economies
            foreach (Settlement settlement in settlements)
            {
                settlement.UpdateEconomy();
            }
            
            // Update all NPCs' daily routines
            // This will be implemented later
        }
    }
} 
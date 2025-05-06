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
        public int startHour = 0;
        public int startMinute = 0;
        
        [Header("Prefabs")]
        public GameObject workAreaPrefab;
        
        [Header("Resource Types")]
        public List<ResourceType> resourceTypes = new List<ResourceType>();
        
        [Header("Settlements")]
        public List<Settlement> settlements = new List<Settlement>();
        
        [Header("NPCs")]
        public List<NPC> npcs = new List<NPC>();
        
        [Header("Player Reference")]
        public GameObject playerCharacter;
        
        private float currentGameTime = 0f;
        public int currentDay = 0;
        
        public float CurrentGameTime => currentGameTime;
        public int CurrentHour => currentTimeToCurrentHour();
        public int CurrentMinute => currentTimeToCurrentMinute(); 
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
            InitializeGameTime();
            InitializeGame();
        }
        
        private void Update()
        {
            UpdateGameTime();
        }

        private void InitializeGameTime () {
            currentGameTime += startHoursAndMinutesToFloat();
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

        private int currentTimeToCurrentHour() {
            return (int)((currentGameTime / dayLength) * 24f);
        }

        private int currentTimeToCurrentMinute() {
            return (int)(((currentGameTime / dayLength) * 24f - CurrentHour) * 60f);
        }

        private float startHoursAndMinutesToFloat() {
            float hoursFloat = (dayLength / 24.0f) * startHour; 
            float minutesFloat = (dayLength / (24.0f * 60.0f)) * startMinute; 
            return hoursFloat + minutesFloat;
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
            resourceTypes.Add(new ResourceType("Processed Meat", "Processed meat from butchering"));
            resourceTypes.Add(new ResourceType("Leather", "Processed leather from tanning"));
            resourceTypes.Add(new ResourceType("Crops", "Harvested crops from farming"));
        }
        
        private void InitializeSettlements()
        {
            // Clear any existing settlements
            settlements.Clear();
        }
        
        private void InitializeNPCs()
        {
            npcs.Clear();
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
           
            // Update all NPCs' daily routines
            // This will be implemented later
        }
    }
} 
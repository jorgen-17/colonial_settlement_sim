using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    public class NPC : MonoBehaviour
    {
        [Header("Basic Info")]
        public string npcName;
        public string occupation;
        public float health = 100f;
        public float energy = 100f;
        public float hunger = 0f;
        public float thirst = 0f;
        
        [Header("Schedule")]
        public float workStartHour = 8f;
        public float workEndHour = 17f;
        public float sleepStartHour = 22f;
        public float sleepEndHour = 6f;
        
        [Header("Inventory")]
        public Dictionary<ResourceType, float> inventory = new Dictionary<ResourceType, float>();
        public float money = 0f;
        
        [Header("Current State")]
        public NPCState currentState = NPCState.Idle;
        public Vector3 currentDestination;
        public WorkArea currentWorkArea;
        
        [Header("Work Route")]
        public List<WorkArea> workRoute = new List<WorkArea>();
        public int currentRouteIndex = 0;
        public float timeSpentAtCurrentArea = 0f;
        public float requiredTimeAtCurrentArea = 0f;
        
        private Settlement homeSettlement;
        private Job currentJob;
        private float currentHour = 0f;
        
        private void Start()
        {
            InitializeNPC();
        }
        
        private void Update()
        {
            UpdateNPCState();
            UpdateNeeds();
            UpdateWorkRoute();
        }
        
        private void InitializeNPC()
        {
            // Initialize inventory
            foreach (ResourceType resource in GameManager.Instance.resourceTypes)
            {
                inventory[resource] = 0f;
            }
            
            // Set initial money
            money = Random.Range(10f, 50f);
            
            // Set up work route based on occupation
            SetupWorkRoute();
        }
        
        private void SetupWorkRoute()
        {
            workRoute.Clear();
            
            switch (occupation.ToLower())
            {
                case "hunter":
                    // Find or create work areas for hunting route
                    WorkArea huntingGround = FindOrCreateWorkArea(WorkAreaType.HuntingGround);
                    WorkArea butcheringStation = FindOrCreateWorkArea(WorkAreaType.ButcheringStation);
                    WorkArea tanningStation = FindOrCreateWorkArea(WorkAreaType.TanningStation);
                    WorkArea market = FindOrCreateWorkArea(WorkAreaType.Market);
                    
                    workRoute.Add(huntingGround);
                    workRoute.Add(butcheringStation);
                    workRoute.Add(tanningStation);
                    workRoute.Add(market);
                    break;
                    
                case "farmer":
                    // Find or create work areas for farming route
                    WorkArea farm = FindOrCreateWorkArea(WorkAreaType.Farm);
                    WorkArea warehouse = FindOrCreateWorkArea(WorkAreaType.Warehouse);
                    WorkArea farmersMarket = FindOrCreateWorkArea(WorkAreaType.Market);
                    
                    workRoute.Add(farm);
                    workRoute.Add(warehouse);
                    workRoute.Add(farmersMarket);
                    break;
                    
                // Add more occupations and their routes here
            }
            
            if (workRoute.Count > 0)
            {
                currentWorkArea = workRoute[0];
                requiredTimeAtCurrentArea = currentWorkArea.processingTime;
            }
        }
        
        private WorkArea FindOrCreateWorkArea(WorkAreaType type)
        {
            // This will be implemented to find existing work areas or create new ones
            // For now, return null
            return null;
        }
        
        private void UpdateWorkRoute()
        {
            if (currentState != NPCState.Working || workRoute.Count == 0)
                return;
            
            if (currentWorkArea == null)
            {
                currentWorkArea = workRoute[0];
                requiredTimeAtCurrentArea = currentWorkArea.processingTime;
                return;
            }
            
            // Update time spent at current area
            timeSpentAtCurrentArea += Time.deltaTime;
            
            // Check if we've spent enough time at the current area
            if (timeSpentAtCurrentArea >= requiredTimeAtCurrentArea)
            {
                // Move to next area in route
                currentRouteIndex = (currentRouteIndex + 1) % workRoute.Count;
                currentWorkArea = workRoute[currentRouteIndex];
                timeSpentAtCurrentArea = 0f;
                requiredTimeAtCurrentArea = currentWorkArea.processingTime;
            }
        }
        
        private void UpdateNPCState()
        {
            currentHour = (GameManager.Instance.CurrentGameTime / GameManager.Instance.dayLength) * 24f;
            
            // Update state based on time of day
            if (currentHour >= sleepStartHour || currentHour < sleepEndHour)
            {
                SetState(NPCState.Sleeping);
            }
            else if (currentHour >= workStartHour && currentHour < workEndHour)
            {
                SetState(NPCState.Working);
            }
            else
            {
                SetState(NPCState.Idle);
            }
        }
        
        private void UpdateNeeds()
        {
            // Increase needs over time
            hunger += Time.deltaTime * 0.1f;
            thirst += Time.deltaTime * 0.15f;
            energy -= Time.deltaTime * 0.05f;
            
            // Clamp values
            hunger = Mathf.Clamp(hunger, 0f, 100f);
            thirst = Mathf.Clamp(thirst, 0f, 100f);
            energy = Mathf.Clamp(energy, 0f, 100f);
            
            // Update health based on needs
            if (hunger > 80f || thirst > 80f || energy < 20f)
            {
                health -= Time.deltaTime * 0.1f;
            }
            else if (hunger < 20f && thirst < 20f && energy > 80f)
            {
                health = Mathf.Min(health + Time.deltaTime * 0.05f, 100f);
            }
        }
        
        public void SetState(NPCState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                OnStateChanged(newState);
            }
        }
        
        private void OnStateChanged(NPCState newState)
        {
            switch (newState)
            {
                case NPCState.Working:
                    // Start work route if not already working
                    if (workRoute.Count > 0)
                    {
                        currentWorkArea = workRoute[0];
                        requiredTimeAtCurrentArea = currentWorkArea.processingTime;
                    }
                    break;
                case NPCState.Sleeping:
                    // Find and go to home
                    GoHome();
                    break;
                case NPCState.Idle:
                    // Find activities to do
                    FindIdleActivity();
                    break;
            }
        }
        
        private void GoHome()
        {
            // Implementation will depend on the NPC's home location
            // This will be implemented later
        }
        
        private void FindIdleActivity()
        {
            // Implementation will include activities like shopping, socializing, etc.
            // This will be implemented later
        }
        
        public void AddToInventory(ResourceType resource, float amount)
        {
            if (inventory.ContainsKey(resource))
            {
                inventory[resource] += amount;
            }
        }
        
        public void RemoveFromInventory(ResourceType resource, float amount)
        {
            if (inventory.ContainsKey(resource))
            {
                inventory[resource] = Mathf.Max(0f, inventory[resource] - amount);
            }
        }
        
        public float GetInventoryAmount(ResourceType resource)
        {
            return inventory.ContainsKey(resource) ? inventory[resource] : 0f;
        }
    }

    public enum NPCState
    {
        Idle,
        Working,
        Sleeping,
        Trading,
        Traveling
    }
} 
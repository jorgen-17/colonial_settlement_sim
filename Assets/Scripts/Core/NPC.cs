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
        public string settlementId;
        
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
        public float moveSpeed = 5f;
        
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
            UpdateMovement();
        }
        
        private void InitializeNPC()
        {
            // Initialize inventory
            foreach (ResourceType resource in GameManager.Instance.resourceTypes)
            {
                inventory[resource] = 0f;
            }
            
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
            // First, try to find an existing work area in the settlement
            Settlement settlement = GameManager.Instance.settlements.Find(s => s.id == settlementId);
            if (settlement == null)
            {
                Debug.LogError($"Settlement with ID {settlementId} not found for NPC {npcName}");
                return null;
            }
            
            WorkArea existingArea = settlement.workAreas.Find(area => area.areaType == type);
            
            if (existingArea != null)
            {
                return existingArea;
            }
            
            // If no existing area found, create a new one
            GameObject workAreaObj = Instantiate(GameManager.Instance.workAreaPrefab);
            WorkArea newArea = workAreaObj.GetComponent<WorkArea>();
            
            // Set up the new work area
            newArea.areaName = $"{type} {settlement.workAreas.Count + 1}";
            newArea.areaType = type;
            newArea.parentSettlement = settlement;
            
            // Position the new work area relative to the settlement
            Vector3 settlementPos = settlement.transform.position;
            Vector3 randomOffset = new Vector3(
                Random.Range(-10f, 10f),
                0f,
                Random.Range(-10f, 10f)
            );
            workAreaObj.transform.position = settlementPos + randomOffset;
            
            // Initialize the work area type-specific properties
            newArea.InitializeWorkAreaType();
            
            // Add the new work area to the settlement
            settlement.workAreas.Add(newArea);
            
            return newArea;
        }
        
        private void UpdateWorkRoute()
        {
            if (currentState != NPCState.Working || workRoute.Count == 0)
                return;
            
            if (currentWorkArea == null)
            {
                currentWorkArea = workRoute[0];
                requiredTimeAtCurrentArea = currentWorkArea.processingTime;
                SetState(NPCState.Traveling);
                return;
            }

            // Only update time if we're actually at the work area
            if (IsAtCurrentWorkArea())
            {
                timeSpentAtCurrentArea += Time.deltaTime;
                
                // Check if we've spent enough time at the current area
                if (timeSpentAtCurrentArea >= requiredTimeAtCurrentArea)
                {
                    // Move to next area in route
                    currentRouteIndex = (currentRouteIndex + 1) % workRoute.Count;
                    currentWorkArea = workRoute[currentRouteIndex];
                    timeSpentAtCurrentArea = 0f;
                    requiredTimeAtCurrentArea = currentWorkArea.processingTime;
                    SetState(NPCState.Traveling);
                }
            }
        }
        
        private void UpdateNPCState()
        {
            currentHour = GameManager.Instance.CurrentHour;
            
            // Update state based on time of day
            if (currentHour >= sleepStartHour || currentHour < sleepEndHour)
            {
                SetState(NPCState.Sleeping);
            }
            else if (currentHour >= workStartHour && currentHour < workEndHour)
            {
                if (IsAtCurrentWorkArea())
                {
                    SetState(NPCState.Working);
                }
                else
                {
                    SetState(NPCState.Traveling);
                }
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

        private bool IsAtCurrentWorkArea()
        {
            if (currentWorkArea == null) return false;
            
            float distance = Vector3.Distance(transform.position, currentWorkArea.transform.position);
            return distance <= 1.5f; // Consider within 2 units as "at" the work area
        }

        private void UpdateMovement()
        {
            if (currentState == NPCState.Traveling && currentWorkArea != null)
            {
                // Move towards current work area
                Vector3 direction = (currentWorkArea.transform.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                
                // Rotate to face movement direction
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
                }
            }
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
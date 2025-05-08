using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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
        public Guid settlementId;
        public Guid id; // Unique identifier for the NPC
        
        [Header("Schedule")]
        public float workStartHour = 8f;
        public float workEndHour = 17f;
        public float sleepStartHour = 22f;
        public float sleepEndHour = 6f;
        
        [Header("Inventory")]
        public List<Resource> inventory = new List<Resource>();
        
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
            // Set up work route based on occupation
            SetupWorkRoute();
        }
        
        private void SetupWorkRoute()
        {
            workRoute.Clear();
            
            switch (occupation.ToLower())
            {
                case "hunter":
                    WorkArea huntingGround = FindWorkArea(WorkAreaType.HuntingGround);
                    WorkArea butcheringStation = FindWorkArea(WorkAreaType.ButcheringStation);
                    WorkArea tanningStation = FindWorkArea(WorkAreaType.TanningStation);
                    WorkArea market = FindWorkArea(WorkAreaType.Market);
                    
                    if (huntingGround != null) workRoute.Add(huntingGround);
                    if (butcheringStation != null) workRoute.Add(butcheringStation);
                    if (tanningStation != null) workRoute.Add(tanningStation);
                    if (market != null) workRoute.Add(market);
                    break;
                    
                case "farmer":
                    WorkArea well = FindWorkArea(WorkAreaType.Well);
                    WorkArea farm = FindWorkArea(WorkAreaType.Farm);
                    WorkArea farmersMarket = FindWorkArea(WorkAreaType.Market);
                    
                    if (well != null) workRoute.Add(well);
                    if (farm != null) workRoute.Add(farm);
                    if (farmersMarket != null) workRoute.Add(farmersMarket);
                    break;
                    
                // Add more occupations and their routes here
            }
            
            if (workRoute.Count > 0)
            {
                currentWorkArea = workRoute[0];
                requiredTimeAtCurrentArea = currentWorkArea.processingTime;
            }
        }
        
        private WorkArea FindWorkArea(WorkAreaType type)
        {
            // First, try to find an existing work area in the settlement
            Settlement settlement = GameManager.Instance.settlements.Find(s => s.id == settlementId);
            if (settlement == null)
            {
                Debug.LogError($"Settlement with ID {settlementId} not found for NPC {npcName}");
                return null;
            }
            
            WorkArea existingArea = settlement.workAreas.Find(area => area.areaType == type);
            
            return existingArea;
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
                // Update time worked at this work area
                float deltaTime = Time.deltaTime;
                currentWorkArea.Work(id, deltaTime);
                
                // Check if we've completed the work at this area
                if (currentWorkArea.GetTimeWorked(id) >= requiredTimeAtCurrentArea)
                {
                    // Complete the work and get resource
                    // todo: need to add resource consumption to work area. e.g. animal carcass -> butchering station -> hide, bone, meat
                    List<Resource> resources = currentWorkArea.FinishWork(id);
                    
                    // Add resource to inventory if we received one
                    if (resources != null)
                    {
                        foreach (Resource resource in resources)
                        {
                            AddToInventory(resource);
                        }
                    }
                    
                    // Move to next area in route
                    currentRouteIndex = (currentRouteIndex + 1) % workRoute.Count;
                    currentWorkArea = workRoute[currentRouteIndex];
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
                    StartWorking();
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

        private void StartWorking() 
        {
            // Start work at current work area if not already working
            if (currentWorkArea != null && IsAtCurrentWorkArea() && !currentWorkArea.IsWorkerAssigned(id))
            {
                var inputResources = FindResourcesInInventory(currentWorkArea.requiredInputs);
                if (currentWorkArea.areaType == WorkAreaType.Market)
                {
                    // items to sell at market
                    inputResources = inventory.Where(item => item.amount > 0 && item.baseValue > 0).ToList(); 
                }

                if (currentWorkArea.StartWork(id, inputResources))
                {
                    RemoveFromInventory(inputResources);
                }
                else
                {
                    // todo: figure out how to reroute NPC to go find missing resources
                    // for now I can just reset them to begginng of their work route
                    currentRouteIndex = 0;
                    currentWorkArea = workRoute[currentRouteIndex];
                    requiredTimeAtCurrentArea = currentWorkArea.processingTime;
                    Debug.LogWarning($"NPC {npcName} failed to start work at {currentWorkArea.areaType}");
                }
            }
        }

        private List<Resource> FindResourcesInInventory(List<Resource> resources)
        {
            return inventory.Where(r => resources.Any(res => res.type == r.type && res.amount <= r.amount)).ToList();
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

        // todo amount not needed its now in the Resource class
        public void AddToInventory(Resource resource)
        {
            Resource existingResource = inventory.Find(r => r.type == resource.type);
            if (existingResource != null)
            {
                existingResource.amount += resource.amount;
            } 
            else
            {
                inventory.Add(resource);
            }
        }

        public void RemoveFromInventory(List<Resource> resources)
        {
            foreach (Resource resource in resources)
            {
                RemoveFromInventory(resource);
            }
        }

        public void RemoveFromInventory(Resource resource)
        {
            Resource existingResource = inventory.Find(r => r.type == resource.type);
            if (existingResource != null)
            {
                existingResource.amount = Mathf.Max(0f, existingResource.amount - resource.amount);
            }
        }
        
        public float GetInventoryAmount(Resource resource)
        {
            return inventory.Find(r => r.type == resource.type)?.amount ?? 0f;
        }

        private bool IsAtCurrentWorkArea()
        {
            if (currentWorkArea == null) return false;
            
            float distance = Vector3.Distance(transform.position, currentWorkArea.transform.position);
            return distance <= 0.5f; // Consider within 0.5 units as "at" the work area
        }

        // Get the time worked at current work area for UI display
        public float GetTimeWorkedAtCurrentArea()
        {
            if (currentWorkArea != null)
            {
                return currentWorkArea.GetTimeWorked(id);
            }
            return 0f;
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
            else if (currentState == NPCState.Working && IsAtCurrentWorkArea())
            {
                // Stop movement and rotation when at work area
                Vector3 workAreaPos = currentWorkArea.transform.position;
                transform.position = workAreaPos;
                transform.rotation = Quaternion.identity;
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
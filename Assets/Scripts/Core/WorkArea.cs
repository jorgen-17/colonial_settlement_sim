using UnityEngine;
using System.Collections.Generic;
using System;

namespace css.core
{
    public class WorkArea : MonoBehaviour
    {
        [Header("Work Area Info")]
        public string areaName;
        public WorkAreaType areaType;
        public Settlement parentSettlement;
        public string outputResource;
        public float outputAmount;
        public float capacity;
        
        [Header("Functionality")]
        public bool isOperational = true;
        public float efficiency = 1f;
        public List<NPC> assignedWorkers = new List<NPC>();
        public int maxWorkers = 1;
        
        [Header("Production")]
        public Dictionary<ResourceType, float> inputResources = new Dictionary<ResourceType, float>();
        public Dictionary<ResourceType, float> outputResources = new Dictionary<ResourceType, float>();
        public float productionRate = 1f;
        
        [Header("Work Area Specific")]
        public float processingTime = 0f; // Time in hours to process inputs into outputs
        public bool requiresTools = false;
        public List<ResourceType> requiredTools = new List<ResourceType>();
        public bool isIndoor = true; // Whether this is an indoor or outdoor work area
        
        // Track worker time
        private Dictionary<Guid, float> workerTime = new Dictionary<Guid, float>();
        
        private void Start()
        {
            InitializeWorkArea();
        }
        
        private void InitializeWorkArea()
        {
            // Initialize resource dictionaries
            foreach (ResourceType resource in GameManager.Instance.resourceTypes)
            {
                inputResources[resource] = 0f;
                outputResources[resource] = 0f;
            }
            
            // Set up work area-specific initialization
            InitializeWorkAreaType();
        }
        
        public void InitializeWorkAreaType()
        {
            switch (areaType)
            {
                case WorkAreaType.HuntingGround:
                    maxWorkers = 2;
                    isIndoor = false;
                    requiresTools = true;
                    processingTime = 20.0f;
                    outputResource = "meat";
                    outputAmount = 5f;
                    requiredTools.Add(new ResourceType("Hunting Tools", "Tools for hunting"));
                    break;
                    
                case WorkAreaType.ButcheringStation:
                    maxWorkers = 2;
                    isIndoor = true;
                    requiresTools = true;
                    processingTime = 15.0f;
                    outputResource = "processed_meat";
                    outputAmount = 3f;
                    requiredTools.Add(new ResourceType("Butchering Tools", "Tools for butchering"));
                    break;
                    
                case WorkAreaType.TanningStation:
                    maxWorkers = 2;
                    isIndoor = true;
                    requiresTools = true;
                    processingTime = 20.0f;
                    outputResource = "leather";
                    outputAmount = 2f;
                    requiredTools.Add(new ResourceType("Tanning Tools", "Tools for tanning"));
                    break;
                    
                case WorkAreaType.Market:
                    maxWorkers = 2;
                    isIndoor = true;
                    processingTime = 10.0f;
                    break;
                    
                case WorkAreaType.Farm:
                    maxWorkers = 4;
                    isIndoor = false;
                    requiresTools = true;
                    processingTime = 30.0f;
                    outputResource = "crops";
                    outputAmount = 10f;
                    requiredTools.Add(new ResourceType("Farming Tools", "Tools for farming"));
                    break;
                    
                case WorkAreaType.Warehouse:
                    maxWorkers = 2;
                    isIndoor = true;
                    processingTime = 5.0f;
                    capacity = 1000f;
                    break;
                    
                case WorkAreaType.House:
                    maxWorkers = 0;
                    isIndoor = true;
                    break;
                    
                case WorkAreaType.Workshop:
                    maxWorkers = 3;
                    isIndoor = true;
                    requiresTools = true;
                    requiredTools.Add(new ResourceType("Tools", "Basic tools for crafting"));
                    break;
                    
                case WorkAreaType.Mine:
                    maxWorkers = 3;
                    isIndoor = false;
                    requiresTools = true;
                    requiredTools.Add(new ResourceType("Mining Tools", "Tools for mining"));
                    break;
            }
        }
        
        public bool AssignWorker(Guid npcId)
        {
            if (workerTime.Count < maxWorkers && !workerTime.ContainsKey(npcId))
            {
                workerTime[npcId] = 0f;
                return true;
            }
            return false;
        }
        
        public void RemoveWorker(Guid npcId)
        {
            if (workerTime.ContainsKey(npcId))
            {
                workerTime.Remove(npcId);
            }
        }
        
        public void Work(Guid npcId, float timeWorked)
        {
            if (workerTime.ContainsKey(npcId))
            {
                workerTime[npcId] += timeWorked;
            }
        }
        
        public ResourceType FinishWork(Guid npcId)
        {
            if (workerTime.ContainsKey(npcId) && workerTime[npcId] >= processingTime)
            {
                // Worker has completed the required time
                workerTime.Remove(npcId);
                
                // TODO: make work area a store of resources which can replenish over time
                return new ResourceType(outputResource, $"{outputResource} resource");
            }
            return null;
        }
        
        public float GetTimeWorked(Guid npcId)
        {
            if (workerTime.ContainsKey(npcId))
            {
                return workerTime[npcId];
            }
            return 0f;
        }
        
        public void UpdateProduction()
        {
            if (!isOperational || workerTime.Count == 0)
                return;
            
            // Calculate actual production rate based on workers and efficiency
            float actualProductionRate = productionRate * efficiency * (float)workerTime.Count / maxWorkers;
            
            // Process inputs and generate outputs
            foreach (var input in inputResources)
            {
                if (input.Value > 0)
                {
                    // Consume input resources
                    inputResources[input.Key] -= actualProductionRate;
                    
                    // Generate output resources based on work area type
                    GenerateOutputs(input.Key, actualProductionRate);
                }
            }
        }
       // TODO: make some work areas probabilistic based on the worker skill and on the quality of tools
       // such as a hunting ground that sometimes doesn't return any resources or you are more likely to
       // get a rabbit but less likely to get a deer. Eventually would be cool to model actual animal 
       // behavior and animal population dynamics.
        private void GenerateOutputs(ResourceType input, float amount)
        {
            // This will be implemented based on specific work area types
            // For example:
            // - HuntingGround: Time -> Animal Carcass
            // - ButcheringStation: Animal Carcass -> Meat, Hide
            // - TanningStation: Hide -> Leather
            // - Workshop: Wood -> Furniture
            // - Farm: Seeds -> Crops
            // - Mine: Time -> Iron Ore
            // This will be implemented later
        }
        
        public bool HasAvailableSpace()
        {
            return workerTime.Count < maxWorkers;
        }
        
        public float GetEfficiency()
        {
            return efficiency * (float)workerTime.Count / maxWorkers;
        }
        
        public bool HasRequiredTools(NPC worker)
        {
            if (!requiresTools) return true;
            
            foreach (ResourceType tool in requiredTools)
            {
                if (worker.GetInventoryAmount(tool) <= 0)
                    return false;
            }
            return true;
        }
    }

    public enum WorkAreaType
    {
        House,
        Market,
        Workshop,
        Warehouse,
        Farm,
        Mine,
        HuntingGround,
        ButcheringStation,
        TanningStation
    }
} 
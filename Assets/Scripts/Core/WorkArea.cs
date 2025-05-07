using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace css.core
{
    public class WorkArea : MonoBehaviour
    {
        [Header("Work Area Info")]
        public Guid id;
        public WorkAreaType areaType;
        public Guid parentSettlementId;
        public List<Resource> requiredInputs;
        public List<Resource> outputResources;
        
        [Header("Functionality")]
        public List<NPC> assignedWorkers = new List<NPC>();
       
        [Header("Work Area Specific")]
        public float processingTime = 0f; // Time in hours to process inputs into outputs

        // Track worker time
        private Dictionary<Guid, float> workerTime = new Dictionary<Guid, float>();
        private List<Resource> inputResources = new List<Resource>();
        
        public void InitializeWorkArea(Guid id, WorkAreaType areaType, Guid settlementId)
        {
            this.id = id;
            this.areaType = areaType;
            this.parentSettlementId = settlementId;

            switch (areaType)
            {
                case WorkAreaType.HuntingGround:
                    processingTime = 20.0f;
                    outputResources = new List<Resource> { ResourceFactory.CreateResource(ResourceType.AnimalCarcass, 1f) };
                    break;
                    
                case WorkAreaType.ButcheringStation:
                    processingTime = 15.0f;
                    requiredInputs = new List<Resource> { ResourceFactory.CreateResource(ResourceType.AnimalCarcass, 1f) };
                    outputResources = new List<Resource> { ResourceFactory.CreateResource(ResourceType.Meat, 5f),
                        ResourceFactory.CreateResource(ResourceType.Hide, 1f) };
                    break;
                    
                case WorkAreaType.TanningStation:
                    processingTime = 20.0f;
                    requiredInputs = new List<Resource> { ResourceFactory.CreateResource(ResourceType.Hide, 1f) };
                    outputResources = new List<Resource> { ResourceFactory.CreateResource(ResourceType.Leather, 1f) };
                    break;
                    
                case WorkAreaType.Market:
                    processingTime = 10.0f;
                    break;

                case WorkAreaType.Well:
                    processingTime = 10.0f;
                    outputResources = new List<Resource> { ResourceFactory.CreateResource(ResourceType.Water, 10f) };
                    break;

                case WorkAreaType.Farm:
                    processingTime = 30.0f;
                    requiredInputs = new List<Resource> { ResourceFactory.CreateResource(ResourceType.Water, 10f),
                        ResourceFactory.CreateResource(ResourceType.Seeds, 1f) };
                    outputResources = new List<Resource> { ResourceFactory.CreateResource(ResourceType.Crops, 10f) };
                    break;
            }
        }

        public bool StartWork(Guid npcId, List<Resource> inputResources)
        {
            if (!CanStartWork(inputResources))
            {
                Debug.Log("Cannot start work, not enough input resources");
                return false;
            }

            if (!workerTime.ContainsKey(npcId))
            {
                this.inputResources = inputResources;
                workerTime[npcId] = 0f;
            }

            return true;
        }

        private bool CanStartWork(List<Resource> inputResources)
        {
            if (requiredInputs == null || requiredInputs.Count == 0)
            {
                return true;
            }
            else if (inputResources == null || inputResources.Count != requiredInputs.Count)
            {
                return false;
            }

            return inputResources.All(r => requiredInputs.Any(ir => ir.type == r.type && ir.amount >= r.amount));
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

        public List<Resource> FinishWork(Guid npcId)
        {
            if (workerTime.ContainsKey(npcId) && workerTime[npcId] >= processingTime)
            {
                // Worker has completed the required time
                workerTime.Remove(npcId);

                if (areaType == WorkAreaType.Market)
                {
                    float amount = inputResources.Aggregate(0f, (sum, resource) => sum + resource.amount * resource.baseValue);
                    return new List<Resource> { ResourceFactory.CreateResource(ResourceType.Gold, amount) };
                }

                // TODO: make work area a store of resources which can replenish over time
                return outputResources;
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
   }

    public enum WorkAreaType
    {
        Market,
        Well,
        Farm,
        HuntingGround,
        ButcheringStation,
        TanningStation
    }
} 
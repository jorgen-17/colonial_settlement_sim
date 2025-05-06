using UnityEngine;
using System.Collections.Generic;
using System;

namespace css.core
{
    public class WorkArea : MonoBehaviour
    {
        [Header("Work Area Info")]
        public Guid id;
        public WorkAreaType areaType;
        public Guid parentSettlementId;
        public string outputResource;
        public float outputAmount;
        
        [Header("Functionality")]
        public List<NPC> assignedWorkers = new List<NPC>();
       
        [Header("Work Area Specific")]
        public float processingTime = 0f; // Time in hours to process inputs into outputs
        
        // Track worker time
        private Dictionary<Guid, float> workerTime = new Dictionary<Guid, float>();
       
        public void InitializeWorkArea(Guid id, WorkAreaType areaType, Guid settlementId)
        {
            this.id = id;
            this.areaType = areaType;
            this.parentSettlementId = settlementId;

            switch (areaType)
            {
                case WorkAreaType.HuntingGround:
                    processingTime = 20.0f;
                    outputResource = "animal_carcass";
                    outputAmount = 1f;
                    break;
                    
                case WorkAreaType.ButcheringStation:
                    processingTime = 15.0f;
                    outputResource = "meat";
                    outputAmount = 3f;
                    break;
                    
                case WorkAreaType.TanningStation:
                    processingTime = 20.0f;
                    outputResource = "leather";
                    outputAmount = 2f;
                    break;
                    
                case WorkAreaType.Market:
                    processingTime = 10.0f;
                    break;
                    
                case WorkAreaType.Farm:
                    processingTime = 30.0f;
                    outputResource = "crops";
                    outputAmount = 10f;
                    break;
            }
        }

        public bool AssignWorker(Guid npcId)
        {
            if (!workerTime.ContainsKey(npcId))
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

        // TODO change ResourceType to Resource
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
   }

    public enum WorkAreaType
    {
        Market,
        Farm,
        HuntingGround,
        ButcheringStation,
        TanningStation
    }
} 
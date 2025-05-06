using UnityEngine;
using System.Collections.Generic;
using System;

namespace css.core
{
    public class Settlement : MonoBehaviour
    {
        [Header("Basic Info")]
        public Guid id;
        public string settlementName;
        public string foundedDate;
        [Header("Population")]
        public int Population => npcs.Count;
        public List<NPC> npcs = new List<NPC>();
        
        [Header("Resources")]
        public Dictionary<ResourceType, float> resources = new Dictionary<ResourceType, float>();
        
        [Header("Economy")]
        public float money = 0f;
        public Dictionary<ResourceType, float> resourcePrices = new Dictionary<ResourceType, float>();
        
        [Header("Work Areas")]
        public List<WorkArea> workAreas = new List<WorkArea>();
        
        private void Start()
        {
            InitializeSettlement();
        }
        
        private void InitializeSettlement()
        {
            // Initialize resources
            foreach (ResourceType resource in GameManager.Instance.resourceTypes)
            {
                resources[resource] = 0f;
                resourcePrices[resource] = resource.baseValue;
            }
        }
        
        public void AddResource(ResourceType resource, float amount)
        {
            if (resources.ContainsKey(resource))
            {
                resources[resource] += amount;
            }
        }
        
        public void RemoveResource(ResourceType resource, float amount)
        {
            if (resources.ContainsKey(resource))
            {
                resources[resource] = Mathf.Max(0f, resources[resource] - amount);
            }
        }
        
        public float GetResourceAmount(ResourceType resource)
        {
            return resources.ContainsKey(resource) ? resources[resource] : 0f;
        }
        
        public float GetResourcePrice(ResourceType resource)
        {
            return resourcePrices.ContainsKey(resource) ? resourcePrices[resource] : resource.baseValue;
        }
        
        public void AddWorkArea(WorkArea workArea)
        {
            if (!workAreas.Contains(workArea))
            {
                workArea.parentSettlementId = id;
                workAreas.Add(workArea);
            }
        }
        
        public void RemoveWorkArea(WorkArea workArea)
        {
            workAreas.Remove(workArea);
        }
    }
} 
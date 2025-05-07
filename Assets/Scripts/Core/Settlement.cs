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
        public Dictionary<Resource, float> resources = new Dictionary<Resource, float>();
        
        [Header("Economy")]
        public float money = 0f;
        public Dictionary<Resource, float> resourcePrices = new Dictionary<Resource, float>();
        
        [Header("Work Areas")]
        public List<WorkArea> workAreas = new List<WorkArea>();
        
        private void Start()
        {
            InitializeSettlement();
        }
        
        private void InitializeSettlement()
        {
            // Initialize resources
            foreach (Resource resource in GameManager.Instance.resources)
            {
                resources[resource] = 0f;
                resourcePrices[resource] = resource.baseValue;
            }
        }
        
        public void AddResource(Resource resource, float amount)
        {
            if (resources.ContainsKey(resource))
            {
                resources[resource] += amount;
            }
        }
        
        public void RemoveResource(Resource resource, float amount)
        {
            if (resources.ContainsKey(resource))
            {
                resources[resource] = Mathf.Max(0f, resources[resource] - amount);
            }
        }
        
        public float GetResourceAmount(Resource resource)
        {
            return resources.ContainsKey(resource) ? resources[resource] : 0f;
        }
        
        public float GetResourcePrice(Resource resource)
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
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace css.core
{
    public class SceneSetup : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject npcPrefab;
        public GameObject workAreaPrefab;
        public GameObject settlementPrefab;

        [Header("Setup Parameters")]
        public string settlementsJsonPath = "Data/settlements.json";

        private void Start()
        {
            SetupGameWorld();
        }

        private void SetupGameWorld()
        {
            // Load settlement data from JSON
            string jsonPath = Path.Combine(Application.dataPath, settlementsJsonPath);
            string jsonContent = File.ReadAllText(jsonPath);
            SettlementData settlementData = JsonConvert.DeserializeObject<SettlementData>(jsonContent);

            // Create each settlement from the data
            foreach (var settlementInfo in settlementData.settlements)
            {
                CreateSettlement(settlementInfo);
            }
        }

        private void CreateSettlement(SettlementInfo settlementInfo)
        {
            // Create settlement object
            Vector3 position = settlementInfo.location.ToVector3();
            GameObject settlementObj = Instantiate(settlementPrefab, position, Quaternion.identity);
            Settlement settlement = settlementObj.GetComponent<Settlement>();
            settlement.id = settlementInfo.id;
            settlement.settlementName = settlementInfo.name;
            settlement.foundedDate = settlementInfo.foundedDate;

            // Create work areas
            foreach (var workAreaData in settlementInfo.workAreas)
            {
                CreateWorkArea(workAreaData, settlement);
            }

            // Create settlers
            foreach (var settlerData in settlementInfo.settlers)
            {
                CreateSettler(settlerData, settlement);
            }

            // Add settlement to GameManager
            GameManager.Instance.settlements.Add(settlement);
        }

        private void CreateWorkArea(WorkAreaData workAreaData, Settlement settlement)
        {
            Vector3 position = workAreaData.location.ToVector3();
            GameObject workAreaObj = Instantiate(workAreaPrefab, position, Quaternion.identity);
            WorkArea workArea = workAreaObj.GetComponent<WorkArea>();
            
            workArea.areaName = workAreaData.id;
            workArea.areaType = (WorkAreaType)System.Enum.Parse(typeof(WorkAreaType), workAreaData.type);
            workArea.processingTime = workAreaData.processingTime;
            workArea.parentSettlement = settlement;

            if (workAreaData.output != null)
            {
                workArea.outputResource = workAreaData.output.resource;
                workArea.outputAmount = workAreaData.output.amount;
            }

            if (workAreaData.capacity > 0)
            {
                workArea.capacity = workAreaData.capacity;
            }

            settlement.workAreas.Add(workArea);
        }

        private void CreateSettler(SettlerData settlerData, Settlement settlement)
        {
            // Position settler near the settlement center
            Vector3 position = settlement.transform.position + new Vector3(
                Random.Range(-5f, 5f),
                0.5f,
                Random.Range(-5f, 5f)
            );

            GameObject npcObj = Instantiate(npcPrefab, position, Quaternion.identity);
            NPC npc = npcObj.GetComponent<NPC>();
            
            npc.npcName = settlerData.name;
            npc.occupation = settlerData.occupation;
            npc.money = settlerData.money;
            npc.settlementId = settlement.id;
            
            // Set schedule
            npc.workStartHour = settlerData.schedule.workStartHour;
            npc.workEndHour = settlerData.schedule.workEndHour;
            npc.sleepStartHour = settlerData.schedule.sleepStartHour;
            npc.sleepEndHour = settlerData.schedule.sleepEndHour;

            // Set inventory
            foreach (var item in settlerData.inventory)
            {
                // Find the matching ResourceType from GameManager's resourceTypes list
                ResourceType resourceType = GameManager.Instance.resourceTypes.Find(rt => rt.name == item.Key);
                if (resourceType != null)
                {
                    npc.AddToInventory(resourceType, item.Value);
                }
                else
                {
                    Debug.LogWarning($"Resource type {item.Key} not found in GameManager's resourceTypes list");
                }
            }

            // Assign to settlement
            settlement.npcs.Add(npc);
        }
    }
}
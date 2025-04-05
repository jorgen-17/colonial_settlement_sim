using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    public class SceneSetup : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject npcPrefab;
        public GameObject workAreaPrefab;
        public GameObject settlementPrefab;

        [Header("Setup Parameters")]
        public int initialNPCs = 5;
        public int initialWorkAreas = 3;
        public Vector3 settlementPosition = new Vector3(0, 0, 0);

        private void Start()
        {
            SetupGameWorld();
        }

        private void SetupGameWorld()
        {
            // Create settlement
            GameObject settlementObj = Instantiate(settlementPrefab, settlementPosition, Quaternion.identity);
            Settlement settlement = settlementObj.GetComponent<Settlement>();
            settlement.settlementName = "New Settlement";
            settlement.population = initialNPCs;

            // Create work areas
            for (int i = 0; i < initialWorkAreas; i++)
            {
                Vector3 position = settlementPosition + new Vector3(
                    Random.Range(-10f, 10f),
                    0,
                    Random.Range(-10f, 10f)
                );

                GameObject workAreaObj = Instantiate(workAreaPrefab, position, Quaternion.identity);
                WorkArea workArea = workAreaObj.GetComponent<WorkArea>();
                workArea.areaName = $"Work Area {i + 1}";
                workArea.areaType = (WorkAreaType)Random.Range(0, System.Enum.GetValues(typeof(WorkAreaType)).Length);
                workArea.parentSettlement = settlement;

                settlement.workAreas.Add(workArea);
            }

            // Create NPCs
            for (int i = 0; i < initialNPCs; i++)
            {
                Vector3 position = settlementPosition + new Vector3(
                    Random.Range(-5f, 5f),
                    0,
                    Random.Range(-5f, 5f)
                );

                GameObject npcObj = Instantiate(npcPrefab, position, Quaternion.identity);
                NPC npc = npcObj.GetComponent<NPC>();
                npc.npcName = $"NPC {i + 1}";
                npc.occupation = GetRandomOccupation();

                // Assign to settlement
                settlement.npcs.Add(npc);
            }

            // Add settlement to GameManager
            GameManager.Instance.settlements.Add(settlement);
        }

        private string GetRandomOccupation()
        {
            string[] occupations = new string[] { "Hunter", "Farmer", "Miner", "Craftsman" };
            return occupations[Random.Range(0, occupations.Length)];
        }
    }
}
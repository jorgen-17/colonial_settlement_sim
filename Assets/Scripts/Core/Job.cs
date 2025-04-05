using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    public class Job
    {
        public string jobTitle;
        public JobType jobType;
        public float baseSalary;
        public List<WorkAreaType> requiredWorkAreas;
        public List<ResourceType> requiredTools;
        public float workEfficiency;
        public float experience;
        public int level;

        public Job(string title, JobType type, float salary)
        {
            jobTitle = title;
            jobType = type;
            baseSalary = salary;
            requiredWorkAreas = new List<WorkAreaType>();
            requiredTools = new List<ResourceType>();
            workEfficiency = 1f;
            experience = 0f;
            level = 1;
        }

        public void AddRequiredWorkArea(WorkAreaType workArea)
        {
            if (!requiredWorkAreas.Contains(workArea))
            {
                requiredWorkAreas.Add(workArea);
            }
        }

        public void AddRequiredTool(ResourceType tool)
        {
            if (!requiredTools.Contains(tool))
            {
                requiredTools.Add(tool);
            }
        }

        public void GainExperience(float amount)
        {
            experience += amount;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            float experienceNeeded = level * 100f; // Simple leveling system
            if (experience >= experienceNeeded)
            {
                level++;
                experience -= experienceNeeded;
                workEfficiency += 0.1f; // 10% efficiency increase per level
            }
        }

        public bool HasRequiredWorkArea(WorkAreaType workArea)
        {
            return requiredWorkAreas.Contains(workArea);
        }

        public bool HasRequiredTool(ResourceType tool)
        {
            return requiredTools.Contains(tool);
        }
    }

    public enum JobType
    {
        Hunter,
        Farmer,
        Miner,
        Craftsman,
        Merchant,
        Laborer,
        Unemployed
    }
} 
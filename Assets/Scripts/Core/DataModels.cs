using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    [System.Serializable]
    public class SettlementData
    {
        public List<SettlementInfo> settlements;
    }

    [System.Serializable]
    public class SettlementInfo
    {
        public string id;
        public string name;
        public Vector3Data location;
        public string foundedDate;
        public int population;
        public Dictionary<string, float> resources;
        public List<WorkAreaData> workAreas;
        public List<SettlerData> settlers;
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float x;
        public float y;
        public float z;

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

    [System.Serializable]
    public class WorkAreaData
    {
        public string id;
        public string type;
        public Vector3Data location;
        public float processingTime;
        public WorkAreaOutput output;
        public float capacity;
    }

    [System.Serializable]
    public class WorkAreaOutput
    {
        public string resource;
        public float amount;
    }

    [System.Serializable]
    public class SettlerData
    {
        public string id;
        public string name;
        public string occupation;
        public float money;
        public Dictionary<string, float> inventory;
        public ScheduleData schedule;
        public string settlementId;
    }

    [System.Serializable]
    public class ScheduleData
    {
        public float workStartHour;
        public float workEndHour;
        public float sleepStartHour;
        public float sleepEndHour;
    }
} 
using System.Collections.Generic;
using System;

namespace css.core
{
    [System.Serializable]
    public class SettlerData
    {
        public string id;
        public string name;
        public string occupation;
        public float money;
        public Dictionary<string, float> inventory;
        public ScheduleData schedule;
        public Guid settlementId;
    }
} 
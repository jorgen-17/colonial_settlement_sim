using System.Collections.Generic;
using System;

namespace css.core
{
    [System.Serializable]
    public class SettlerData
    {
        public Guid id;
        public string name;
        public string occupation;
        public Dictionary<ResourceType, float> inventory;
        public ScheduleData schedule;
        public Guid settlementId;
    }
} 
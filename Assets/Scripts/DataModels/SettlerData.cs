using System.Collections.Generic;

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
        public string settlementId;
    }
} 
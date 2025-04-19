using System.Collections.Generic;
using System;

namespace css.core
{
    [System.Serializable]
    public class SettlementInfo
    {
        public Guid id;
        public string name;
        public Vector3Data location;
        public string foundedDate;
        public Dictionary<string, float> resources;
        public List<WorkAreaData> workAreas;
        public List<SettlerData> settlers;
    }
} 
using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    [System.Serializable]
    public class SettlementInfo
    {
        public string id;
        public string name;
        public Vector3Data location;
        public string foundedDate;
        public Dictionary<string, float> resources;
        public List<WorkAreaData> workAreas;
        public List<SettlerData> settlers;
    }
} 
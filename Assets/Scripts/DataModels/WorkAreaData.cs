using UnityEngine;

namespace css.core
{
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
} 
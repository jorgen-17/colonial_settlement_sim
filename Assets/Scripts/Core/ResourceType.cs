using UnityEngine;

namespace css.core
{
    [System.Serializable]
    public class ResourceType
    {
        public string name;
        public string description;
        public float baseValue;
        public float weight;
        public bool isRawMaterial;
        
        public ResourceType(string name, string description, float baseValue = 1f, float weight = 1f, bool isRawMaterial = true)
        {
            this.name = name;
            this.description = description;
            this.baseValue = baseValue;
            this.weight = weight;
            this.isRawMaterial = isRawMaterial;
        }
    }
} 
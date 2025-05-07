namespace css.core
{
    [System.Serializable]
    public class Resource
    {
        public ResourceType type;
        public float baseValue;
        public float weight;
        public float amount;
        
        public Resource(ResourceType type, float baseValue = 1f, float weight = 1f, float amount = 1f)
        {
            this.type = type;
            this.baseValue = baseValue;
            this.weight = weight;
            this.amount = amount;
        }
    }

    public enum ResourceType
    {
        AnimalCarcass,
        Meat,
        Hide,
        Leather,
        Crops,
        Seeds,
        Water,
        Gold
    }
} 
namespace css.core
{
    public class ResourceFactory
    {
        public static Resource CreateResource(ResourceType type, float amount = 1f)
        {
            switch (type)
            {
                case ResourceType.AnimalCarcass:
                    return new Resource(type, 2f, 100f, amount);
                case ResourceType.Meat:
                    return new Resource(type, 0.1f, 1f, amount);
                case ResourceType.Hide:
                    return new Resource(type, 0.5f, 3f, amount);
                case ResourceType.Leather:
                    return new Resource(type, 3f, 2f, amount);
                case ResourceType.Crops:
                    return new Resource(type, 1f, 1f, amount);
                case ResourceType.Seeds:
                    return new Resource(type, 0.5f, 0.5f, amount);
                case ResourceType.Water:
                    return new Resource(type, 0.1f, 1f, amount);
                case ResourceType.Gold:
                    return new Resource(type, 100f, 1f, amount);
                default:
                    return null;
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    public class Caravan : MonoBehaviour
    {
        [Header("Caravan Info")]
        public string caravanName;
        public Settlement originSettlement;
        public Settlement destinationSettlement;
        public float travelSpeed = 5f;
        public float maxLoad = 1000f;
        
        [Header("Cargo")]
        public Dictionary<ResourceType, float> cargo = new Dictionary<ResourceType, float>();
        public float currentLoad = 0f;
        
        [Header("Route")]
        public List<Vector3> routePoints = new List<Vector3>();
        public int currentRouteIndex = 0;
        public float distanceToNextPoint;
        
        private void Start()
        {
            InitializeCaravan();
        }
        
        private void Update()
        {
            if (routePoints.Count > 0)
            {
                MoveAlongRoute();
            }
        }
        
        private void InitializeCaravan()
        {
            // Initialize cargo dictionary
            foreach (ResourceType resource in GameManager.Instance.resourceTypes)
            {
                cargo[resource] = 0f;
            }
            
            // Calculate initial route
            CalculateRoute();
        }
        
        private void CalculateRoute()
        {
            // This will be implemented to calculate the best route between settlements
            // This will be implemented later
        }
        
        private void MoveAlongRoute()
        {
            if (currentRouteIndex < routePoints.Count - 1)
            {
                Vector3 currentPoint = routePoints[currentRouteIndex];
                Vector3 nextPoint = routePoints[currentRouteIndex + 1];
                
                // Move towards next point
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    nextPoint,
                    travelSpeed * Time.deltaTime
                );
                
                // Check if we've reached the next point
                distanceToNextPoint = Vector3.Distance(transform.position, nextPoint);
                if (distanceToNextPoint < 0.1f)
                {
                    currentRouteIndex++;
                    
                    // If we've reached the destination
                    if (currentRouteIndex >= routePoints.Count - 1)
                    {
                        ArriveAtDestination();
                    }
                }
            }
        }
        
        private void ArriveAtDestination()
        {
            // Unload cargo at destination
            UnloadCargo();
            
            // Calculate new route back to origin
            SwapOriginAndDestination();
            CalculateRoute();
        }
        
        private void SwapOriginAndDestination()
        {
            Settlement temp = originSettlement;
            originSettlement = destinationSettlement;
            destinationSettlement = temp;
        }
        
        public void LoadCargo(ResourceType resource, float amount)
        {
            if (cargo.ContainsKey(resource))
            {
                float newLoad = currentLoad + (amount * resource.weight);
                if (newLoad <= maxLoad)
                {
                    cargo[resource] += amount;
                    currentLoad = newLoad;
                }
            }
        }
        
        private void UnloadCargo()
        {
            foreach (var item in cargo)
            {
                if (item.Value > 0)
                {
                    destinationSettlement.AddResource(item.Key, item.Value);
                    cargo[item.Key] = 0f;
                }
            }
            currentLoad = 0f;
        }
        
        public void UpdateRoute()
        {
            // This will be implemented to update the route based on current market conditions
            // This will be implemented later
        }
        
        public float GetCargoAmount(ResourceType resource)
        {
            return cargo.ContainsKey(resource) ? cargo[resource] : 0f;
        }
        
        public float GetAvailableSpace()
        {
            return maxLoad - currentLoad;
        }
    }
} 
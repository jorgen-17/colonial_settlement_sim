using UnityEngine;
using System.Collections.Generic;

namespace css.core
{
    public class Market : MonoBehaviour
    {
        [Header("Market Info")]
        public string marketName;
        public Settlement parentSettlement;
        
        [Header("Trading")]
        public Dictionary<ResourceType, float> currentPrices = new Dictionary<ResourceType, float>();
        public List<TradeOffer> activeOffers = new List<TradeOffer>();
        
        [Header("Market Hours")]
        public float openHour = 6f;
        public float closeHour = 20f;
        
        private void Start()
        {
            InitializeMarket();
        }
        
        private void InitializeMarket()
        {
            // Initialize prices based on settlement's economy
            foreach (ResourceType resource in GameManager.Instance.resourceTypes)
            {
                currentPrices[resource] = parentSettlement.GetResourcePrice(resource);
            }
        }
        
        public void UpdatePrices(Dictionary<ResourceType, float> newPrices)
        {
            foreach (var price in newPrices)
            {
                currentPrices[price.Key] = price.Value;
            }
            
            // Update active offers based on new prices
            UpdateActiveOffers();
        }
        
        private void UpdateActiveOffers()
        {
            foreach (TradeOffer offer in activeOffers)
            {
                // Adjust offer prices based on current market prices
                offer.UpdatePrice(currentPrices[offer.resource]);
            }
        }
        
        public bool IsOpen()
        {
            float currentHour = (GameManager.Instance.CurrentGameTime / GameManager.Instance.dayLength) * 24f;
            return currentHour >= openHour && currentHour < closeHour;
        }
        
        public void AddTradeOffer(TradeOffer offer)
        {
            activeOffers.Add(offer);
            // Notify nearby NPCs about the new offer
            NotifyNearbyNPCs(offer);
        }
        
        public void RemoveTradeOffer(TradeOffer offer)
        {
            activeOffers.Remove(offer);
        }
        
        private void NotifyNearbyNPCs(TradeOffer offer)
        {
            // This will be implemented to notify NPCs about new trading opportunities
            // This will be implemented later
        }
        
        public float GetCurrentPrice(ResourceType resource)
        {
            return currentPrices.ContainsKey(resource) ? currentPrices[resource] : resource.baseValue;
        }
    }

    [System.Serializable]
    public class TradeOffer
    {
        public NPC seller;
        public ResourceType resource;
        public float amount;
        public float pricePerUnit;
        public bool isSelling; // true for selling, false for buying
        
        public TradeOffer(NPC seller, ResourceType resource, float amount, float pricePerUnit, bool isSelling)
        {
            this.seller = seller;
            this.resource = resource;
            this.amount = amount;
            this.pricePerUnit = pricePerUnit;
            this.isSelling = isSelling;
        }
        
        public void UpdatePrice(float newPrice)
        {
            pricePerUnit = newPrice;
        }
        
        public float GetTotalValue()
        {
            return amount * pricePerUnit;
        }
    }
} 
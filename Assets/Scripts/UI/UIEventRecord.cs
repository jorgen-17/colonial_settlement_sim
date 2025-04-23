using System;

namespace css.ui
{
    // Struct to represent a UI event with all possible parameters
    public class UIEventRecord
    {
        public enum EventType
        {
            PageChange,
            SettlementDetail
        }
        
        public EventType Type { get; private set; }
        public string PageName { get; private set; }
        public Guid SettlementId { get; private set; }
        public DateTime Timestamp { get; private set; }
        
        // Constructor for page change events
        public static UIEventRecord CreatePageChangeEvent(string pageName)
        {
            return new UIEventRecord 
            {
                Type = EventType.PageChange,
                PageName = pageName,
                SettlementId = Guid.Empty,
                Timestamp = DateTime.Now
            };
        }
        
        // Constructor for settlement detail events
        public static UIEventRecord CreateSettlementDetailEvent(Guid settlementId)
        {
            return new UIEventRecord
            {
                Type = EventType.SettlementDetail,
                PageName = null,
                SettlementId = settlementId,
                Timestamp = DateTime.Now
            };
        }
        
        public override string ToString()
        {
            switch (Type)
            {
                case EventType.PageChange:
                    return $"[{Timestamp:HH:mm:ss}] Page Change: {PageName}";
                case EventType.SettlementDetail:
                    return $"[{Timestamp:HH:mm:ss}] Settlement Detail: {SettlementId}";
                default:
                    return "Unknown Event";
            }
        }
    }
} 
using System;

namespace css.ui
{
    // Struct to represent a UI event with all possible parameters
    public class UIEventRecord
    {
        public enum EventType
        {
            PageChange,
            SettlementDetail,
            NPCDetail
        }
        
        public EventType Type { get; private set; }
        public string PageName { get; private set; }
        public Guid SettlementId { get; private set; }
        public Guid NPCId { get; private set; }
        public DateTime Timestamp { get; private set; }
        
        // Constructor for page change events
        public static UIEventRecord CreatePageChangeEvent(string pageName)
        {
            return new UIEventRecord 
            {
                Type = EventType.PageChange,
                PageName = pageName,
                SettlementId = Guid.Empty,
                NPCId = Guid.Empty,
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
                NPCId = Guid.Empty,
                Timestamp = DateTime.Now
            };
        }
        
        // Constructor for NPC detail events
        public static UIEventRecord CreateNPCDetailEvent(Guid npcId)
        {
            return new UIEventRecord
            {
                Type = EventType.NPCDetail,
                PageName = null,
                SettlementId = Guid.Empty,
                NPCId = npcId,
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
                case EventType.NPCDetail:
                    return $"[{Timestamp:HH:mm:ss}] NPC Detail: {NPCId}";
                default:
                    return "Unknown Event";
            }
        }
    }
} 
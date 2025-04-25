using System;

namespace css.ui
{
    // Base class for UI events
    public abstract class UIEventRecord
    {
        public DateTime Timestamp { get; protected set; }
        
        protected UIEventRecord()
        {
            Timestamp = DateTime.Now;
        }
        
        public abstract override string ToString();
    }
    
    // Page change event type
    public class PageChangeEvent : UIEventRecord
    {
        public string PageName { get; private set; }
        
        public PageChangeEvent(string pageName)
        {
            PageName = pageName;
        }
        
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] Page Change: {PageName}";
        }
    }
    
    // Settlement detail event type
    public class SettlementDetailEvent : UIEventRecord
    {
        public Guid SettlementId { get; private set; }
        
        public SettlementDetailEvent(Guid settlementId)
        {
            SettlementId = settlementId;
        }
        
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] Settlement Detail: {SettlementId}";
        }
    }
    
    // NPC detail event type
    public class NPCDetailEvent : UIEventRecord
    {
        public Guid NPCId { get; private set; }
        
        public NPCDetailEvent(Guid npcId)
        {
            NPCId = npcId;
        }
        
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] NPC Detail: {NPCId}";
        }
    }
} 
using UnityEngine;
using System;

namespace css.ui
{
    // Static class for UI events that can be accessed from anywhere
    public static class UIEvents
    {
        // Generic event for page navigation
        public static event Action<string> OnPageChangeRequested;

        // Specific event for navigating to settlement details
        public static event Action<Guid> OnSettlementDetailRequested;

        // Method to trigger the page change event
        public static void RequestPageChange(string pageName)
        {
            OnPageChangeRequested?.Invoke(pageName);
        }

        // Method to trigger navigation to settlement details
        public static void RequestSettlementDetail(Guid settlementId)
        {
            OnSettlementDetailRequested?.Invoke(settlementId);
        }
    }
} 
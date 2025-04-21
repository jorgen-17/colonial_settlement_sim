using UnityEngine;

namespace css.ui
{
    /// <summary>
    /// Interface for all menu pages in the game
    /// </summary>
    public interface MenuPage
    {
        /// <summary>
        /// Initialize the page with parent transform and gameObject
        /// </summary>
        void Initialize(Transform parent, GameObject pageGameObject);
        
        /// <summary>
        /// Update the page content
        /// </summary>
        void Update();
        
        /// <summary>
        /// Handle mouse click events
        /// </summary>
        void HandleMouseClick(Vector2 mousePosition);
        
        /// <summary>
        /// Show the page
        /// </summary>
        void Show();
        
        /// <summary>
        /// Hide the page
        /// </summary>
        void Hide();
        
        /// <summary>
        /// Check if the page is active
        /// </summary>
        bool IsActive();
    }
} 
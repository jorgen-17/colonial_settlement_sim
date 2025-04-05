using UnityEngine;

namespace css.core
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float sprintSpeed = 8f;
        public float jumpForce = 5f;
        
        [Header("Look Settings")]
        public float mouseSensitivity = 2f;
        public float maxLookAngle = 80f;
        
        [Header("References")]
        public Camera playerCamera;
        
        private float verticalRotation = 0f;
        private Rigidbody rb;
        private bool isGrounded;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            
            // Lock and hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // If no camera is assigned, try to find it
            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();
        }
        
        private void Update()
        {
            HandleMouseLook();
            HandleJump();
        }
        
        private void FixedUpdate()
        {
            HandleMovement();
        }
        
        private void HandleMouseLook()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            // Rotate the camera for looking up/down
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            
            // Rotate the player for looking left/right
            transform.Rotate(Vector3.up * mouseX);
        }
        
        private void HandleMovement()
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            
            // Get input axes
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            // Calculate movement direction relative to where we're looking
            Vector3 movement = transform.right * horizontal + transform.forward * vertical;
            movement = movement.normalized * currentSpeed;
            
            // Apply movement
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
        
        private void HandleJump()
        {
            // Simple ground check
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
            
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
        // Toggle cursor lock when pressing Escape
        private void OnApplicationFocus(bool hasFocus)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? 
                    CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = !Cursor.visible;
            }
        }
    }
}
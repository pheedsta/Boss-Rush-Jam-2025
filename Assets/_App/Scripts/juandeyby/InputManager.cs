using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class InputManager : MonoBehaviour
    {
        private InputActions _inputActions;
        
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Vector2 movementInput;
        [SerializeField] private Vector2 cameraInput;
        [SerializeField] private float verticalInput;
        public float VerticalInput => verticalInput;
        [SerializeField] private float horizontalInput;
        public float HorizontalInput => horizontalInput;
        [SerializeField] private float cameraInputX;
        public float CameraInputX => cameraInputX;
        [SerializeField] private float cameraInputY;
        public float CameraInputY => cameraInputY;
        private float _moveAmount;

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputActions();
                _inputActions.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
                _inputActions.Player.Move.canceled += i => movementInput = Vector2.zero;
                
                _inputActions.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
                _inputActions.Player.Look.canceled += i => cameraInput = Vector2.zero;
            }
            _inputActions.Enable();
        }
        
        private void OnDisable()
        {
            _inputActions.Disable();
        }
        
        public void HandleAllInputs()
        {
            HandleMovementInput();
            // HandleJumpInput();
            // HandleAttackInput();
        }
        
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            
            cameraInputX = cameraInput.x;
            cameraInputY = cameraInput.y;
            
            _moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            playerAnimator.UpdateAnimatorValues(0, _moveAmount);
        }
    }
}
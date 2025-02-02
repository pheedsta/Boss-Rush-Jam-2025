using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class InputManager : MonoBehaviour
    {
        private InputActions _inputActions;
        
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private PlayerLocomotion playerLocomotion;
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

        [SerializeField] private bool jumpInput;
        [SerializeField] private bool attackInput;
        [SerializeField] private bool specialInput;

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputActions();
                _inputActions.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
                _inputActions.Player.Move.canceled += i => movementInput = Vector2.zero;
                
                _inputActions.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
                _inputActions.Player.Look.canceled += i => cameraInput = Vector2.zero;
                
                _inputActions.Player.Jump.performed += i => jumpInput = true;
                _inputActions.Player.Attack.performed += i => attackInput = true;
                _inputActions.Player.Special.performed += i => specialInput = true;
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
            HandleJumpInput();
            HandleAttackInput();
            HandleSpecialInput();
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
        
        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                playerLocomotion.HandleJumping();
            }
        }
        
        private void HandleAttackInput()
        {
            if (attackInput)
            {
                attackInput = false;
                playerLocomotion.HandleAttack();
            }
        }
        
        private void HandleSpecialInput()
        {
            if (specialInput)
            {
                specialInput = false;
                playerLocomotion.HandleSpecial();
            }
        }
    }
}
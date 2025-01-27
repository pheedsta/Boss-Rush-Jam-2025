using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float movementSpeed = 5f;
        private Camera _camera;
        private Vector3 _moveDirection;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void HandleAllMovement()
        {
            HandleMovement();
            HandleRotation();
        }
        
        private void HandleMovement()
        {
            _moveDirection = _camera.transform.forward * inputManager.VerticalInput;
            _moveDirection += _camera.transform.right * inputManager.HorizontalInput;
            _moveDirection.Normalize();
            _moveDirection.y = 0;
            _moveDirection *= movementSpeed;

            var movementVelocity = _moveDirection;
            rb.linearVelocity = movementVelocity;
        }
        
        private void HandleRotation()
        {
            var lookDirection = _camera.transform.forward * inputManager.VerticalInput;
            lookDirection += _camera.transform.right * inputManager.HorizontalInput;
            lookDirection.Normalize();
            lookDirection.y = 0;

            if (lookDirection == Vector3.zero)
            {
                lookDirection = transform.forward;
            }
            
            var targetRotation = Quaternion.LookRotation(lookDirection);
            var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
            
            rb.rotation = playerRotation;
        }
    }
}

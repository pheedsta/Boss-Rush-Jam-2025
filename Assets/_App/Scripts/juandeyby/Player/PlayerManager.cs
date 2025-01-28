using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerLocomotion playerLocomotion;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private bool isInteracting;
        public bool IsInteracting => isInteracting;

        private void Update()
        {
            inputManager.HandleAllInputs();
        }
        
        private void FixedUpdate()
        {
            playerLocomotion.HandleAllMovement();
        }

        private void LateUpdate()
        {
            cameraManager.HandleAllCameraMovement();
            
            isInteracting = animator.GetBool("IsInteracting");
            playerLocomotion.IsJumping = animator.GetBool("IsJumping");
            animator.SetBool("IsGrounded", playerLocomotion.IsGrounded);
        }
    }
}

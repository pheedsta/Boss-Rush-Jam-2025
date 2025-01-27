using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerLocomotion playerLocomotion;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private CameraManager cameraManager;

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
        }
    }
}

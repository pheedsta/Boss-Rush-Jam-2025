using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Transform cameraPivot; // The object camera uses to pivot (look up and down)
        [SerializeField] private Transform targetTransform; // The object the camera follows
        [SerializeField] private LayerMask collisionLayers; // The layer the camera will collide with
        private Transform _cameraTransform; // The camera itself
        private float _defaultPosition;
        private Vector3 _cameraFollowVelocity = Vector3.zero;
        private Vector3 _cameraVectorPosition;
        
        [SerializeField] private float cameraCollisionOffset = 0.2f; // How far the camera will jump back when colliding
        [SerializeField] private float minCollisionOffset = 0.2f; // The minimum distance the camera will jump back
        [SerializeField] private float cameraCollisionRadius = 2f;
        [SerializeField] private float cameraFollowSpeed = 0.2f;
        [SerializeField] private float cameraLookSpeed = 2f;
        [SerializeField] private float cameraPivotSpeed = 2f;
        
        [SerializeField] private float lookAngle;
        [SerializeField] private float pivotAngle;
        [SerializeField] private float minPivotAngle = -20f;
        [SerializeField] private float maxPivotAngle = 30f;

        private void Awake()
        {
            if (Camera.main != null) _cameraTransform = Camera.main.transform;
            _defaultPosition = cameraPivot.localPosition.z;
        }

        public void HandleAllCameraMovement()
        {
            FollowTarget();
            RotateCamera(lookAngle);
            HandleCameraCollision();
        }
        
        private void FollowTarget()
        {
            var targetPosition = Vector3.SmoothDamp(
                transform.position, targetTransform.position, ref _cameraFollowVelocity, cameraFollowSpeed);
            transform.position = targetPosition;
        }
        
        private void RotateCamera(float angle)
        {
            Vector3 rotation;
            Quaternion targetRotation;
            
            lookAngle += inputManager.CameraInputX * cameraLookSpeed;
            pivotAngle -= inputManager.CameraInputY * cameraPivotSpeed;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);
            
            rotation = Vector3.zero;
            rotation.y = lookAngle;
            targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;
            
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivot.localRotation = targetRotation;
        }
        
        private void HandleCameraCollision()
        {
            var targetPosition = _defaultPosition;
            RaycastHit hit;
            var direction = _cameraTransform.position - cameraPivot.position;
            direction.Normalize();
            
            if (Physics.SphereCast(
                    cameraPivot.transform.position, cameraCollisionRadius,
                    direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
            {
                var hitDistance = Vector3.Distance(cameraPivot.position, hit.point);
                targetPosition =- hitDistance - cameraCollisionOffset;
            }

            if (Mathf.Abs(targetPosition) < minCollisionOffset)
            {
                targetPosition -= minCollisionOffset;
            }
            
            _cameraVectorPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, targetPosition, 0.2f);
            _cameraTransform.localPosition = _cameraVectorPosition;
        }
    }
}

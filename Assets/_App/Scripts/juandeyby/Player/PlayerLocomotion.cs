using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private PlayerSpell playerSpell;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float movementSpeed = 5f;
        private Camera _camera;
        private Vector3 _moveDirection;
        
        [Header("Falling")]
        [SerializeField] float inAirTimer;
        [SerializeField] float fallingVelocity = 33f;
        [SerializeField] float leapingVelocity = 3f;
        [SerializeField] float rayCastHeightOffset = 0.5f;
        [SerializeField] LayerMask groundLayer;
        
        [Header("Movement Flags")]
        [SerializeField] bool isGrounded = true;
        public bool IsGrounded => isGrounded;
        [SerializeField] bool isJumping;
        public bool IsJumping { get => isJumping; set => isJumping = value; }

        [Header("Jump Speeds")]
        [SerializeField] float jumpHeight = 3f;
        
        [Header("Poison Stroke")]
        private Coroutine _poisonStrokeCoroutine;
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        public void HandleAllMovement()
        {
            HandleFallingAndLanding();
            if (playerManager.IsInteracting) return;
            HandleMovement();
            HandleRotation();
        }
        
        private void HandleMovement()
        {
            if (isJumping) return;
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
            if (isJumping) return;
            var lookDirection = _camera.transform.forward * inputManager.VerticalInput;
            lookDirection += _camera.transform.right * inputManager.HorizontalInput;
            lookDirection.Normalize();
            lookDirection.y = 0;

            if (lookDirection == Vector3.zero)
            {
                lookDirection = transform.forward;
            }
            
            var targetRotation = Quaternion.LookRotation(lookDirection);
            Debug.DrawRay(transform.position, targetRotation * Vector3.forward * 10f, Color.blue);
            var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            Debug.DrawRay(transform.position, playerRotation * Vector3.forward * 10f, Color.green);
            rb.MoveRotation(targetRotation);
        }
        
        private void HandleFallingAndLanding()
        {
            Debug.Log("HandleFallingAndLanding");
            var rayCastOrigin = transform.position;
            rayCastOrigin.y += rayCastHeightOffset;
            
            if (!isGrounded && !isJumping)
            {
                if (!playerManager.IsInteracting)
                {
                    playerAnimator.PlayTargetAnimation("Falling", true);
                }
                inAirTimer += Time.deltaTime;
                rb.AddForce(transform.forward * leapingVelocity);
                rb.AddForce(Vector3.down * fallingVelocity * inAirTimer);
            }
            
            Debug.DrawRay(rayCastOrigin, Vector3.down, Color.red);
            if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out var hit, 1, groundLayer))
            {
                if (!isGrounded && !playerManager.IsInteracting)
                {
                    playerAnimator.PlayTargetAnimation("Land", true);
                }
                inAirTimer = 0;
                if (isGrounded == false) transform.SetParent(hit.transform);
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        public void HandleJumping()
        {
            if (isGrounded)
            {
                playerAnimator.Animator.SetBool("IsJumping", true);
                playerAnimator.PlayTargetAnimation("Jump", false);
                
                ServiceLocator.Get<MusicManager>().Jump();
                
                var jumpingVelocity = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
                var playerVelocity = _moveDirection;
                playerVelocity.y = jumpingVelocity;
                rb.linearVelocity = playerVelocity;
                
                transform.SetParent(null);
            }
        }
        
        public void HandleAttack()
        {
            if (isJumping) return;
            if (isGrounded)
            {
                var randomAttack = UnityEngine.Random.Range(0, 3);
                switch (randomAttack)
                {
                    case 0:
                        playerAnimator.PlayTargetAnimation("Attack1", true);
                        break;
                    case 1:
                        playerAnimator.PlayTargetAnimation("Attack2", true);
                        break;
                    case 2:
                        playerAnimator.PlayTargetAnimation("Attack3", true);
                        break;
                }
                
                // Play sound effect
                ServiceLocator.Get<MusicManager>().PlaySwordWhoosh();
            }
        }

        public void HandleSpecial()
        {
            if (isJumping) return;
            if (isGrounded && playerSpell.CanCastSpell())
            {
                playerAnimator.PlayTargetAnimation("RangeAttack", true);
            }
        }
        
        public void Stroke(Vector3 direction, float force)
        {
            Debug.Log("Stroke!");
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
        
        public void Poison()
        {
            if (_poisonStrokeCoroutine != null)
            {
                StopCoroutine(_poisonStrokeCoroutine);
            }
            _poisonStrokeCoroutine = StartCoroutine(OnPoisonCoroutine());
        }

        private IEnumerator OnPoisonCoroutine()
        {
            var poisonTime = 5f;
            var elapsedTime = 0f;
            movementSpeed = 3f;
            ServiceLocator.Get<MusicManager>().PlaySizzle();
            while (elapsedTime < poisonTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            ServiceLocator.Get<MusicManager>().StopSizzle();
            movementSpeed = 7.5f;
        }
    }
}

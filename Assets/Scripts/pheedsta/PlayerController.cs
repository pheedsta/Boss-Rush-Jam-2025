using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: PlayerController
//++++++++++++++++++++++++++++++++++++++++//

public class PlayerController : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float strafeSpeed = 10f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float aimSpeed = 50f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpVelocity = 5f;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://

    private Transform _bodyTransform;
    private Transform _cameraTransform;
    private CharacterController _characterController;

    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private bool _isMoving;
    private bool _isLooking;
    private bool _jumpPerformed;

    private Vector2 _lookDelta;
    private Vector2 _moveDelta;
    
    private Vector3 _cameraRotation = Vector3.zero;
    private Vector3 _playerVelocity = Vector3.zero;

    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://
    
    private void Awake() {
        // get Transforms
        _bodyTransform = transform.Find("Body");
        _cameraTransform = transform.Find("Body/Camera");
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_bodyTransform, "Body Transform is missing");
        Debug.Assert(_cameraTransform, "Camera Transform is missing");
        //++++++++++++++++++++++++++++++++++++++++//
        
        // get Components
        _characterController = GetComponent<CharacterController>();
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_characterController, "CharacterController Component is missing");
        //++++++++++++++++++++++++++++++++++++++++//
    }

    private void OnEnable() {
        InputManager.OnMove += InputManager_OnMove;
        InputManager.OnLook += InputManager_OnLook;
        InputManager.OnJump += InputManager_OnJump;
    }

    private void Update() {
        // get grounded state once before move calls
        var isGrounded = _characterController.isGrounded;
        
        // process player inputs
        Look();
        Rotate();
        Move();
        Jump(isGrounded);
    }

    private void OnDisable() {
        InputManager.OnJump -= InputManager_OnJump;
    }

    //:::::::::::::::::::::::::::::://
    // Movement & Rotation
    //:::::::::::::::::::::::::::::://

    private void Look() {
        // if playing is not looking we're done
        if (!_isLooking) return;
        
        // update camera rotation values (clamping to min / max)
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x + _lookDelta.y * aimSpeed * Time.deltaTime, -50f, 50f);
        
        // rotate camera around x axis (up / down)
        _cameraTransform.localEulerAngles = _cameraRotation;
    }

    private void Rotate() {
        // if playing is not looking we're done
        if (!_isLooking) return;
        
        // rotate body around the y axis
        _bodyTransform.Rotate(Vector3.up, _lookDelta.x * turnSpeed * Time.deltaTime);
    }

    private void Move() {
        // if player isn't moving we're done
        if (!_isMoving) return;
        
        // calculate forward and strafe vectors
        var forward = _moveDelta.y * moveSpeed * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.forward);
        var strafe = _moveDelta.x * strafeSpeed * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.right);
        
        // move player (X and Z axis)
        _characterController.Move(forward + strafe);
    }

    private void Jump(bool isGrounded) {
        if (isGrounded) {
            // player is grounded
            if (_jumpPerformed) {
                // jump button pressed; set velocity to maximum
                _playerVelocity.y = jumpVelocity;
            } else if (_playerVelocity.y < 0f) {
                // player is falling; reset velocity to zero
                _playerVelocity.y = 0f;
            }
        }
        
        // factor in gravity to jump velocity (we need to always do this so isGrounded works correctly)
        _playerVelocity.y += Constant.Value.Gravity * Time.deltaTime;

        // move player (Y axis)
        _characterController.Move(_playerVelocity * Time.deltaTime);
        
        // reset jump flag
        _jumpPerformed = false;
    }
    
    //:::::::::::::::::::::::::::::://
    // InputManager Events
    //:::::::::::::::::::::::::::::://

    private void InputManager_OnMove(InputManager.ActionPhase phase, Vector2 value) {
        _isMoving = phase != InputManager.ActionPhase.Canceled;
        _moveDelta = value;
    }

    private void InputManager_OnLook(InputManager.ActionPhase phase, Vector2 value) {
        _isLooking = phase != InputManager.ActionPhase.Canceled;
        _lookDelta = value;
    }
    
    private void InputManager_OnJump(InputManager.ActionPhase phase) {
        _jumpPerformed = phase == InputManager.ActionPhase.Performed;
    }
}
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
    // Managers
    //:::::::::::::::::::::::::::::://
    
    private InputManager _inputManager;

    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private bool _jumpPerformed;
    
    private Vector3 _cameraRotation = Vector3.zero;
    private Vector3 _playerVelocity = Vector3.zero;

    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnEnable() {
        InputManager.OnJump += InputManager_OnJump;
    }

    private void Start() {
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
        
        // get managers
        _inputManager = InputManager.Instance;
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_inputManager, "InputManager is missing");
        //++++++++++++++++++++++++++++++++++++++++//
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
        // update camera rotation values (clamping to min / max)
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x + _inputManager.LookDelta.y * aimSpeed * Time.deltaTime, -50f, 50f);
        
        // rotate camera around x axis (up / down)
        _cameraTransform.localEulerAngles = _cameraRotation;
    }

    private void Rotate() {
        // rotate body around the y axis
        _bodyTransform.Rotate(Vector3.up, _inputManager.LookDelta.x * turnSpeed * Time.deltaTime);
    }

    private void Move() {
        // get move delta from InputManager
        var moveDelta = _inputManager.MoveDelta;

        // calculate forward and strafe vectors
        var forward = moveDelta.y * moveSpeed * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.forward);
        var strafe = moveDelta.x * strafeSpeed * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.right);
        
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

    private void InputManager_OnJump() {
        _jumpPerformed = true;
    }
}
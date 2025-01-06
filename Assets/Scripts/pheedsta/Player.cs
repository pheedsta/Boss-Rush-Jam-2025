using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Player
//++++++++++++++++++++++++++++++++++++++++//

public class Player : Character {
    
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

    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private bool _jump;
    private Vector2 _lookDelta;
    private Vector2 _moveDelta;
    private Vector3 _cameraRotation = Vector3.zero;

    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected override void Awake() {
        base.Awake();
        
        // get Transforms
        _bodyTransform = transform.Find("Body");
        _cameraTransform = transform.Find("Body/Camera");
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_bodyTransform, "Body Transform is missing");
        Debug.Assert(_cameraTransform, "Camera Transform is missing");
        //++++++++++++++++++++++++++++++++++++++++//
    }

    private void OnEnable() {
        // subscribe to InputManager events
        InputManager.OnMove += InputManager_OnMove;
        InputManager.OnLook += InputManager_OnLook;
        InputManager.OnJump += InputManager_OnJump;
    }

    protected override void Update() {
        base.Update();
        
        // process player inputs
        Look();
        Rotate();
        Move();
        Jump();
    }

    private void OnDisable() {
        // unsubscribe from InputManager events
        InputManager.OnMove -= InputManager_OnMove;
        InputManager.OnLook -= InputManager_OnLook;
        InputManager.OnJump -= InputManager_OnJump;
    }

    //:::::::::::::::::::::::::::::://
    // Movement & Rotation
    //:::::::::::::::::::::::::::::://

    private void Look() {
        // update camera rotation values (clamping to min / max)
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x + _lookDelta.y * aimSpeed * Time.deltaTime, -50f, 50f);
        
        // rotate camera around x axis (up / down)
        _cameraTransform.localEulerAngles = _cameraRotation;
    }

    private void Rotate() {
        // rotate body around the y axis
        _bodyTransform.Rotate(Vector3.up, _lookDelta.x * turnSpeed * Time.deltaTime);
    }

    private void Move() {
        // add forward and strafe motions
        AddMotion(_moveDelta.y * moveSpeed * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.forward));
        AddMotion(_moveDelta.x * strafeSpeed * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.right));
    }

    private void Jump() {
        // if jump performed this frame AND player is grounded, add jump
        if (_jump && IsGrounded) AddJump(jumpVelocity);
        
        // reset field
        _jump = false;
    }
    
    //:::::::::::::::::::::::::::::://
    // InputManager Events
    //:::::::::::::::::::::::::::::://

    private void InputManager_OnMove(InputManager.ActionPhase phase, Vector2 value) {
        _moveDelta = value;
    }

    private void InputManager_OnLook(InputManager.ActionPhase phase, Vector2 value) {
        _lookDelta = value;
    }
    
    private void InputManager_OnJump(InputManager.ActionPhase phase) {
        _jump = phase == InputManager.ActionPhase.Performed;
    }
}
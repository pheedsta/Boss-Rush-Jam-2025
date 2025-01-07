using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Player
//++++++++++++++++++++++++++++++++++++++++//

public class Player : Character {
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_SpeedMargin = 0.1f; // a margin to prevent constant speed calculations when player is almost at speed
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Moving")]
    [Tooltip("Walk speed of the player in m/s")]
    [SerializeField] private float walkSpeed = 10f;
    [Tooltip("Sprint speed of the player in m/s")]
    [SerializeField] private float sprintSpeed = 20f;
    [Tooltip("How fast the player accelerates")]
    [SerializeField] private float moveAccelerationRate = 10f;
    
    [Header("Strafing")]
    [Tooltip("Strafe speed of the player in m/s")]
    [SerializeField] private float strafeSpeed = 10f;
    [Tooltip("How fast the player accelerates")]
    [SerializeField] private float strafeAccelerationRate = 10f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 1.5f;
    
    [Header("Turning & Aiming")]
    [Tooltip("Rotation speed of the player")]
    [SerializeField] private float turnSpeed = 100f;
    [Range(-10f, -90f)]
    [Tooltip("How far in degrees the aim camera moves down")]
    [SerializeField] private float minAimDegrees = -80.0f;
    [Range(10f, 90f)]
    [Tooltip("How far in degrees the aim camera moves up")]
    [SerializeField] private float maxAimDegrees = 80.0f;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://

    private Transform _bodyTransform;
    private Transform _cameraTransform;

    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private bool _jump;
    private bool _sprint;
    private Vector2 _lookDelta;
    private Vector2 _moveDelta;
    private Vector3 _cameraRotation;
    private Vector3 _horizontalVelocity;

    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected override void Awake() {
        base.Awake();
        
        // get Transforms
        _bodyTransform = transform.Find("Body");
        _cameraTransform = transform.Find("Camera Root");
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_bodyTransform, "'Body' Transform is missing");
        Debug.Assert(_cameraTransform, "'Camera Root' Transform is missing");
        //++++++++++++++++++++++++++++++++++++++++//
    }

    private void OnEnable() {
        // subscribe to InputManager events
        InputManager.OnMove += InputManager_OnMove;
        InputManager.OnLook += InputManager_OnLook;
        InputManager.OnJump += InputManager_OnJump;
        InputManager.OnSprint += InputManager_OnSprint;
    }

    protected override void Update() {
        // process player inputs
        Rotate();
        Move();
        Jump();
        
        // NB: always call this last so it can action the movements above on this frame
        base.Update();
    }

    private void LateUpdate() {
        // process camera inputs
        Look();
    }

    private void OnDisable() {
        // unsubscribe from InputManager events
        InputManager.OnMove -= InputManager_OnMove;
        InputManager.OnLook -= InputManager_OnLook;
        InputManager.OnJump -= InputManager_OnJump;
        InputManager.OnSprint -= InputManager_OnSprint;
    }

    //:::::::::::::::::::::::::::::://
    // Movement & Rotation
    //:::::::::::::::::::::::::::::://

    private void Look() {
        // update camera rotation values (clamping to min / max)
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x + _lookDelta.y * turnSpeed * Time.deltaTime, minAimDegrees, maxAimDegrees);
        
        // rotate camera around x axis (up / down)
        _cameraTransform.localEulerAngles = _cameraRotation;
    }

    private void Rotate() {
        // rotate body around the y axis
        AddRotation(_lookDelta.x * turnSpeed * Time.deltaTime);
    }

    private void Move() {
        // get target move and strafe speeds
        var targetMoveSpeed = _moveDelta.y * (_sprint ? sprintSpeed : walkSpeed);
        var targetStrafeSpeed = _moveDelta.x * strafeSpeed;
        
        if (_horizontalVelocity.y < targetMoveSpeed - k_SpeedMargin || _horizontalVelocity.y > targetMoveSpeed + k_SpeedMargin) {
            // player is outside target move margin, calculate speed using acceleration
            _horizontalVelocity.y = Mathf.Lerp(_horizontalVelocity.y, targetMoveSpeed, moveAccelerationRate * Time.deltaTime);
        } else {
            // player is inside target move margin, set speed to target speed
            _horizontalVelocity.y = targetMoveSpeed;
        }
        
        if (_horizontalVelocity.x < targetStrafeSpeed - k_SpeedMargin || _horizontalVelocity.x > targetStrafeSpeed + k_SpeedMargin) {
            // player is outside target move margin, calculate speed using acceleration
            _horizontalVelocity.x = Mathf.Lerp(_horizontalVelocity.x, targetStrafeSpeed, strafeAccelerationRate * Time.deltaTime);
        } else {
            // player is inside target move margin, set speed to target speed
            _horizontalVelocity.x = targetStrafeSpeed;
        }
        
        // add move and strafe motions
        AddMotion(_horizontalVelocity.y * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.forward));
        AddMotion(_horizontalVelocity.x * Time.deltaTime * _bodyTransform.TransformDirection(Vector3.right));
    }

    private void Jump() {
        // if jump hasn't been pressed; we're done
        if (!_jump) return;
        
        // reset field
        _jump = false;
        
        // calculate jump velocity using desired jump height as basis (DON'T USE Time.deltaTime)
        // NB: this will check if character is grounded
        AddJump(Mathf.Sqrt(jumpHeight * -2f * gravity));
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
    
    private void InputManager_OnSprint(InputManager.ActionPhase phase) {
        _sprint = phase == InputManager.ActionPhase.Performed;
    }
}
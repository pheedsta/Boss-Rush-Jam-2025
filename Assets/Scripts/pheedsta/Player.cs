using _App.Scripts.juandeyby;
using UnityEngine;

//------------------------------//
// Required Components
//------------------------------//

[RequireComponent(typeof(Health))]

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Player
//++++++++++++++++++++++++++++++++++++++++//

public class Player : Character {
    
    //------------------------------//
    // Static Properties 
    //------------------------------//

    public static Player Instance;
    
    //------------------------------//
    // Properties
    //------------------------------//

    public Health Health => GetHealth();
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_InputThreshold = 0.01f; // if input is below this threshold; make it zero 
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Character States")]
    [SerializeField] private PlayerStateScript idleStateScript;
    [SerializeField] private PlayerStateScript moveStateScript;
    [SerializeField] private PlayerStateScript dashStateScript;
    [SerializeField] private PlayerStateScript attackStateScript;
    [SerializeField] private PlayerStateScript specialStateScript;
    [SerializeField] private PlayerStateScript dieStateScript;
    
    [Header("Moving")]
    [Tooltip("Walk speed of the player in m/s")]
    [SerializeField] private float walkSpeed = 10f;
    [Tooltip("How fast the player accelerates")]
    [SerializeField] private float accelerationRate = 10f;
    [Tooltip("How fast the player decelerates")]
    [SerializeField] private float decelerationRate = 10f;
    
    [Header("Dashing")]
    [Tooltip("How far the player dashes in meters")]
    [SerializeField] private float dashDistance = 10f;
    [Tooltip("How many seconds the dash takes")]
    [SerializeField] private float dashDuration = 0.5f;
    [Tooltip("How many seconds before the player can dash again")]
    [SerializeField] private float dashCooldown = 2f;
    
    [Header("Aiming")]
    [SerializeField] private float mouseAimSpeed = 5f;
    [SerializeField] private float gamepadAimSpeed = 3f;
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://
    
    private UIManager UIManager => GetUIManager();
    
    //:::::::::::::::::::::::::::::://
    // Character States
    //:::::::::::::::::::::::::::::://

    private CharacterState _idleState;
    private CharacterState _moveState;
    private CharacterState _dashState;
    private CharacterState _attackState;
    private CharacterState _specialState;
    private CharacterState _dieState;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private Camera _camera;
    private Health _health;
    private Animator _animator;
    private UIManager _uiManager;
    
    //:::::::::::::::::::::::::::::://
    // Transforms
    //:::::::::::::::::::::::::::::://

    private Transform _aimPoint;
    private Transform _movePoint;
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private float _dashTimer;
    private float _dashSpeed;
    
    private Plane _plane;
    private Vector2 _lookDelta;
    private Vector2 _moveDelta;
    private Vector3 _velocity;
    
    private InputManager.InputDevice _lookDevice;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected override void Awake() {
        base.Awake();

        // singleton
        if (Instance) {
            Destroy(gameObject);
        } else {
            Instance = this;
            Configure();
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        
        // register Player with ComponentRegistry
        ComponentRegistry.Register(this);
        
        // subscribe to InputManager events
        InputManager.OnMove += InputManager_OnMove;
        InputManager.OnLook += InputManager_OnLook;
        InputManager.OnDash += InputManager_OnDash;
        InputManager.OnAttack += InputManager_OnAttack;
        InputManager.OnSpecial += InputManager_OnSpecial;
        
        // subscribe to CharacterHealth events
        Health.OnChange += Health_OnChange;
        
        // reset transforms to defaults
        _aimPoint.rotation = Quaternion.identity;
        _movePoint.rotation = Quaternion.identity;
        
        // reset fields to defaults
        _velocity = Vector3.zero;
        _dashTimer = dashCooldown; // this allows player to dash immediately
    }

    protected override void Update() {
        base.Update();
        
        // process player inputs
        Look();
        Move();
        Dash();
        
        // increment dash timer
        _dashTimer += Time.deltaTime;
    }

    protected override void OnDisable() {
        base.OnDisable();
        
        // deregister from ComponentRegistry
        ComponentRegistry.Deregister(this);
        
        // unsubscribe from InputManager events
        InputManager.OnMove -= InputManager_OnMove;
        InputManager.OnLook -= InputManager_OnLook;
        InputManager.OnDash -= InputManager_OnDash;
        InputManager.OnAttack -= InputManager_OnAttack;
        InputManager.OnSpecial -= InputManager_OnSpecial;
        
        // unsubscribe from CharacterHealth events
        Health.OnChange -= Health_OnChange;
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // get required components (these won't be null)
        _health = GetComponent<Health>();
        
        // get camera
        _camera = Camera.main;
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_camera, "Main Camera is null");
        //++++++++++++++++++++++++++++++//
        
        // get components
        _animator = transform.GetComponentInChildren<Animator>();
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_animator, "Animator component is null");
        //++++++++++++++++++++++++++++++//
        
        // get transforms
        _aimPoint = transform.Find("Aim Point");
        _movePoint = transform.Find("Move Point");
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_aimPoint, "'Aim Point' transform is null");
        Debug.Assert(_movePoint, "'Move Point' transform is null");
        //++++++++++++++++++++++++++++++//
        
        // initialise private CharacterStates
        _idleState = new CharacterState(this, Instantiate(idleStateScript));
        _moveState = new CharacterState(this, Instantiate(moveStateScript));
        _dashState = new CharacterState(this, Instantiate(dashStateScript));
        _attackState = new CharacterState(this, Instantiate(attackStateScript));
        _specialState = new CharacterState(this, Instantiate(specialStateScript));
        _dieState = new CharacterState(this, Instantiate(dieStateScript));

        // initialise reference plane for ray casting
        _plane = new Plane(Vector3.up, Vector3.zero);
        
        // calculate dash speed once
        _dashSpeed = dashDistance / dashDuration;
    }
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private Health GetHealth() {
        // if Health component has already been found, we're done
        if (_health) return _health;
        
        // Health is a required component so it will never be null
        _health = GetComponent<Health>();
        
        // return Health component
        return _health;
    }

    private UIManager GetUIManager() {
        // if UIManager has already been found, we're done
        if (_uiManager) return _uiManager;
        
        // get UIManager from UIServiceLocator
        _uiManager = UIServiceLocator.Get<UIManager>();
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_uiManager, "UIManager is null");
        //++++++++++++++++++++++++++++++//
        
        // return UIManager
        return _uiManager;
    }
    
    //:::::::::::::::::::::::::::::://
    // Health Events
    //:::::::::::::::::::::::::::::://

    private void Health_OnChange(int health) {
        // update PlayerHealth bar
        UIManager.HubPanel.PlayerHealth.SetHealth(Health.HealthPercentage);
    }

    //:::::::::::::::::::::::::::::://
    // Look
    //:::::::::::::::::::::::::::::://

    private void Look() {
        if (_lookDevice == InputManager.InputDevice.Mouse) {
            LookMouse(_lookDelta);
        } else {
            LookGamepad(_lookDelta);
        }
    }

    private void LookMouse(Vector2 input) {
        // get ray from camera to mouse position
        var ray = _camera.ScreenPointToRay(input);
        
        // if raycast onto plane fails; we're done (this should not happen)
        if (!_plane.Raycast(ray, out var enter)) return;
        
        // calculate rotation needed to look at mouse point
        var focusPositionOnPlane = _plane.ClosestPointOnPlane(_aimPoint.position);
        var rotation = Quaternion.LookRotation(ray.GetPoint(enter) - focusPositionOnPlane);

        // rotate towards point using mouse aim speed
        _aimPoint.rotation = Quaternion.Slerp(_aimPoint.rotation, rotation, mouseAimSpeed * Time.deltaTime);
    }

    private void LookGamepad(Vector2 input) {
        // if input is zero, we're done
        if (input == Vector2.zero) return;
        
        // convert input to Vector3
        var direction = new Vector3(input.x, 0f, input.y);
        
        // calculate rotation
        var rotation = Quaternion.LookRotation(direction);

        // rotate towards point using gamepad aim speed
        _aimPoint.rotation = Quaternion.Slerp(_aimPoint.rotation, rotation, gamepadAimSpeed * Time.deltaTime);
    }

    //:::::::::::::::::::::::::::::://
    // Move & Dash
    //:::::::::::::::::::::::::::::://
    
    private void Move() {
        if (_moveDelta != Vector2.zero) {
            // if move delta is not zero; rotate move point to the input direction
            // this allows player to dash in the direction they were last moving
            var direction = new Vector3(_moveDelta.x, 0f, _moveDelta.y);
            _movePoint.rotation = Quaternion.LookRotation(direction);
        }
        
        // if player is dashing we're done
        if (_dashTimer <= dashDuration) return;
        
        // calculate deceleration delta for this frame once
        var decelerationDelta = decelerationRate * Time.deltaTime;
        
        if (Mathf.Abs(_moveDelta.x) < k_InputThreshold) {
            // x input is less than threshold, decelerate towards zero
            if (Mathf.Abs(_velocity.x) < decelerationDelta) {
                // absolute velocity (x) is less than the amount we will decelerate; just set it to zero
                _velocity.x = 0f;
            } else {
                // move towards zero
                _velocity.x -= Mathf.Sign(_velocity.x) * decelerationDelta;
            }
        } else {
            // x input is greater than threshold, accelerate towards max speed
            _velocity.x += _moveDelta.x * accelerationRate * Time.deltaTime;
        }
        
        if (Mathf.Abs(_moveDelta.y) < k_InputThreshold) {
            // y input is less than threshold, decelerate towards zero
            if (Mathf.Abs(_velocity.z) < decelerationDelta) {
                // absolute velocity (z) is less than the amount we will decelerate; just set it to zero
                _velocity.z = 0f;
            } else {
                // move towards zero
                _velocity.z -= Mathf.Sign(_velocity.z) * decelerationDelta;
            }
        } else {
            // y input is greater than threshold, accelerate towards max speed
            _velocity.z += _moveDelta.y * accelerationRate * Time.deltaTime;
        }
        
        // cap velocity to move speed
        if (_velocity.magnitude > walkSpeed) _velocity = _velocity.normalized * walkSpeed;
        
        // add motion to player
        AddMotion(_velocity * Time.deltaTime);
    }

    private void Dash() {
        // if player is dash, dash in the direction they're moving
        if (_dashTimer <= dashDuration) AddMotion(_dashSpeed * Time.deltaTime * _movePoint.forward);
    }
    
    //:::::::::::::::::::::::::::::://
    // InputManager Events
    //:::::::::::::::::::::::::::::://

    private void InputManager_OnMove(InputManager.ActionPhase phase, InputManager.InputDevice inputDevice, Vector2 value) {
        _moveDelta = inputDevice == InputManager.InputDevice.Keyboard ? DigitiseKeyboardInput(value) : value;
    }

    private void InputManager_OnLook(InputManager.ActionPhase phase, InputManager.InputDevice inputDevice, Vector2 value) {
        _lookDevice = inputDevice;
        _lookDelta = value;
    }
    
    private void InputManager_OnDash(InputManager.ActionPhase phase) {
        // if dash was not performed OR dash is on cooldown, we're done
        if (phase != InputManager.ActionPhase.Performed || _dashTimer < dashCooldown) return;
        
        // reset dash timer
        _dashTimer = 0f;
    }
    
    private void InputManager_OnAttack(InputManager.ActionPhase phase) {
    }
    
    private void InputManager_OnSpecial(InputManager.ActionPhase phase) {
    }
    
    //:::::::::::::::::::::::::::::://
    // Utilities
    //:::::::::::::::::::::::::::::://

    private static Vector2 DigitiseKeyboardInput(Vector2 input) {
        // initialise field
        var digitisedInput = Vector2.zero;

        // digitise X value
        digitisedInput.x = input.x switch {
            < -0.5f => -1f,
            > 0.5f => 1f,
            _ => 0f
        };

        // digitise Y value
        digitisedInput.y = input.y switch {
            < -0.5f => -1f,
            > 0.5f => 1f,
            _ => 0f
        };

        // return newly digitised vector
        return digitisedInput;
    }
}
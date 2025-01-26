using _App.Scripts.juandeyby;
using UnityEngine;
using UnityEngine.Serialization;

//------------------------------//
// Required Components
//------------------------------//

[RequireComponent(typeof(Health))]

//++++++++++++++++++++++++++++++//
// CLASS: Player
//++++++++++++++++++++++++++++++//

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

    private const float k_InputThreshold = 0.01f; // a margin to determine if player is inputting movement
    private const float k_SpeedMargin = 0.1f; // a margin to prevent constant speed calculations when player is almost at speed
    
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
    
    [Header("Turning")]
    [Tooltip("Rotation speed of the player")]
    [SerializeField] private float turnSpeed = 100f;
    
    [Header("Moving")]
    [Tooltip("Walk speed of the player in m/s")]
    [SerializeField] private float walkSpeed = 10f;
    [Tooltip("How fast the player accelerates")]
    [SerializeField] private float accelerationRate = 10f;
    [Tooltip("How fast the player decelerates")]
    [SerializeField] private float decelerationRate = 10f;
    
    [Header("Animation")]
    [SerializeField] PlayerAnimator playerAnimator;
    
    /*[Header("Dashing")]
    [Tooltip("How far the player dashes in meters")]
    [SerializeField] private float dashDistance = 10f;
    [Tooltip("How many seconds the dash takes")]
    [SerializeField] private float dashDuration = 0.5f;
    [Tooltip("How many seconds before the player can dash again")]
    [SerializeField] private float dashCooldown = 2f;*/
    
    [Header("Jumping")]
    [SerializeField] private float jumpVelocity = 5f;
    //[SerializeField] private float jumpHeight = 1.5f;
    
    [Header("Shards")]
    [Tooltip("How many fire shards to collect to fully charge fire ability")]
    [SerializeField] private int minimumFireShards = 10;
    
    [Header("Camera")]
    [SerializeField] private CameraTarget cameraTarget;
    
    [Header("Prefabs")]
    [SerializeField] private Projectile projectilePrefab;
    
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
    
    private Health _health;
    private UIManager _uiManager;
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private int _fireShardCount;
    private bool _isAiming;
    
    //private float _dashTimer;
    //private float _dashSpeed;
    
    private Vector2 _moveDelta;
    private Vector3 _moveVelocity;
    
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
        InputManager.OnJump += InputManager_OnJump;
        //InputManager.OnDash += InputManager_OnDash;
        InputManager.OnAttack += InputManager_OnAttack;
        InputManager.OnSpecial += InputManager_OnSpecial;
        
        // subscribe to CharacterHealth events
        Health.OnChange += Health_OnChange;
        
        // reset fields to defaults
        _isAiming = false;
        _moveVelocity = Vector3.zero;
        //_dashTimer = dashCooldown; // this allows player to dash immediately
    }

    protected override void Start() {
        base.Start();
        
        // update health and special ability UI
        UpdateHealthUI();
        UpdateSpecialAbilityUI();
    }

    protected override void Update() {
        base.Update();
        
        // process player inputs
        Move();
        
        // increment dash timer
        //_dashTimer += Time.deltaTime;
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        
        // move camera target to follow player
        MoveCameraTarget();
    }

    protected override void OnDisable() {
        base.OnDisable();
        
        // deregister from ComponentRegistry
        ComponentRegistry.Deregister(this);
        
        // unsubscribe from InputManager events
        InputManager.OnMove -= InputManager_OnMove;
        //InputManager.OnDash -= InputManager_OnDash;
        InputManager.OnAttack -= InputManager_OnAttack;
        InputManager.OnSpecial -= InputManager_OnSpecial;
        
        // unsubscribe from CharacterHealth events
        Health.OnChange -= Health_OnChange;
    }
    
    //------------------------------//
    // Collectables
    //------------------------------//

    public void Collect(Collectable collectable) {
        switch (collectable) {
            case CollectableShard:
                // increment shard count and update special ability UI
                _fireShardCount++;
                UpdateSpecialAbilityUI();
                break;
            case CollectableHeart heart:
                // apply heal
                Health.ApplyHeal(heart.Health);
                break;
        }
    }

    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        //++++++++++++++++++++++++++++++//
        Debug.Assert(cameraTarget, "'Camera Target' is not set");
        //++++++++++++++++++++++++++++++//
        
        // get required components (these won't be null)
        _health = GetComponent<Health>();
        
        // initialise private CharacterStates
        _idleState = new CharacterState(this, Instantiate(idleStateScript));
        _moveState = new CharacterState(this, Instantiate(moveStateScript));
        _dashState = new CharacterState(this, Instantiate(dashStateScript));
        _attackState = new CharacterState(this, Instantiate(attackStateScript));
        _specialState = new CharacterState(this, Instantiate(specialStateScript));
        _dieState = new CharacterState(this, Instantiate(dieStateScript));
        
        // calculate dash speed once
        //_dashSpeed = dashDistance / dashDuration;
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
        UpdateHealthUI();
    }
    
    //:::::::::::::::::::::::::::::://
    // UI Methods
    //:::::::::::::::::::::::::::::://

    private void UpdateHealthUI() {
        UIManager.HubPanel.PlayerHealth.SetHealth(Health.HealthPercentage);
    }

    private void UpdateSpecialAbilityUI() {
        UIManager.HubPanel.PlayerSingleAbility.SetCooldownOverlay(Mathf.Clamp((float)_fireShardCount / (float)minimumFireShards, 0f, 1f));
    }

    //:::::::::::::::::::::::::::::://
    // Moving
    //:::::::::::::::::::::::::::::://
    
    private void Move() {
        
        if (Mathf.Abs(_moveDelta.x) < k_InputThreshold && Mathf.Abs(_moveDelta.y) < k_InputThreshold) {
            // no move input; we want to slow the velocity (using deceleration rate)
            // if move velocity is less than threshold; reset to zero than target move speed; cap move velocity 
            _moveVelocity -= decelerationRate * Time.deltaTime * _moveVelocity.normalized;
            if (_moveVelocity.magnitude < 0.1f) _moveVelocity = Vector3.zero;
        } else {
            // get target move speed
            //var targetMoveSpeed = _dashTimer <= dashDuration ? _dashSpeed : walkSpeed;
            //var targetAcceleration = _dashTimer <= dashDuration ? accelerationRate * (_dashSpeed / walkSpeed) : accelerationRate;
            
            // get the forward vector of the camera target
            var cameraForward = cameraTarget.transform.forward;
            var cameraRight = cameraTarget.transform.right;

            // exclude pitch rotation from vectors
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // calculate direction using camera direction and user input
            var direction = _moveDelta.y * cameraForward + _moveDelta.x * cameraRight;

            // update move velocity (using acceleration rate)
            // if move velocity is faster than target move speed; cap move velocity 
            _moveVelocity += accelerationRate * Time.deltaTime * direction;
            if (_moveVelocity.magnitude > walkSpeed) _moveVelocity = _moveVelocity.normalized * walkSpeed;

            if (!_isAiming) {
                // player is not aiming, rotate player towards new move direction (using turn speed)
                var rotation = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Time.deltaTime, 0f);
                transform.rotation = Quaternion.LookRotation(rotation);
            }
        }
        
        // move the player
        AddMotion(_moveVelocity * Time.deltaTime);
        
        // if player is not aiming, we're done
        if (!_isAiming) return;
        
        // player is aiming, get camera target rotation (excluding y rotation)
        var cameraRotation = cameraTarget.transform.forward;
        cameraRotation.y = 0f;
        
        // rotate the player in the direction of the camera
        transform.rotation = Quaternion.LookRotation(cameraRotation);
    }
    
    //:::::::::::::::::::::::::::::://
    // Camera Target
    //:::::::::::::::::::::::::::::://

    private void MoveCameraTarget() {
        // get the camera target position and update x and z values
        var position = cameraTarget.transform.position;
        position.x = transform.position.x;
        position.y = transform.position.y + 1.5f;
        position.z = transform.position.z;

        // update position
        cameraTarget.transform.position = position;
    }
    
    //:::::::::::::::::::::::::::::://
    // InputManager Events
    //:::::::::::::::::::::::::::::://

    private void InputManager_OnMove(InputManager.ActionPhase phase, InputManager.InputDevice inputDevice, Vector2 value) {
        _moveDelta = inputDevice == InputManager.InputDevice.Keyboard ? DigitiseKeyboardInput(value) : value;
    }

    private void InputManager_OnJump(InputManager.ActionPhase phase) {
        if (phase == InputManager.ActionPhase.Performed) AddJump(jumpVelocity);
    }
    
    /*private void InputManager_OnDash(InputManager.ActionPhase phase) {
        // if dash was not performed OR dash is on cooldown, we're done
        if (phase != InputManager.ActionPhase.Performed || _dashTimer < dashCooldown) return;
        
        // reset dash timer
        _dashTimer = 0f;
    }*/
    
    private void InputManager_OnAttack(InputManager.ActionPhase phase) {
    }
    
    private void InputManager_OnSpecial(InputManager.ActionPhase phase) {
        if (phase == InputManager.ActionPhase.Canceled) {
            // special button released; reset field
            _isAiming = false;
            
            // hide the crosshairs
            CrosshairCanvas.Instance.HideCrosshairs();
            
            // stop camera target from aiming
            cameraTarget.StopAiming();

            // if we don't have enough fire shards, we're done
            if (_fireShardCount < minimumFireShards) return;
            
            // decrement shard count and update UI
            _fireShardCount -= minimumFireShards;
            UpdateSpecialAbilityUI();

            // if there is no fire ability prefab; we're done
            if (!projectilePrefab) return;
        
            // get direction of the fireball (depending on where camera is looking)
            var rotation = Quaternion.LookRotation(cameraTarget.transform.forward);
        
            // instantiate fireball
            _ = ReusablePool.FetchReusable(projectilePrefab, cameraTarget.transform.position, rotation);
        } else {
            // special button pressed; update field
            _isAiming = true;
            
            // show the crosshairs
            CrosshairCanvas.Instance.ShowCrosshairs();
            
            // start camera target from aiming
            cameraTarget.StartAiming();
        }
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
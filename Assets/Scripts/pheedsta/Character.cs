using UnityEngine;

//------------------------------//
// Required Components
//------------------------------//

[RequireComponent(typeof(CharacterController))]

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Character
//++++++++++++++++++++++++++++++++++++++++//

public class Character : MonoBehaviour {
    
    //------------------------------//
    // Properties
    //------------------------------//
    
    public CharacterStateMachine StateMachine { get; private set; }
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_GroundedOffset = 0.15f;
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_MaxGroundedGravityY = -2f; // using -2f instead of zero stops Character 'bouncing' down ramps
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Physics")]
    [Tooltip("The character uses its own gravity value (default is -9.81f)")]
    [SerializeField] protected float gravity = -9.81f;
    [Tooltip("What layers the character uses as ground")]
    [SerializeField] private LayerMask groundLayers;
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://
    
    private bool IsGrounded => _groundCollider;
    private float GroundOverlapSphereRadius => _characterController.radius;
    private Vector3 GroundOverlapSpherePosition => GetGroundOverlapSpherePosition();
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private CharacterController _characterController;
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly Collider[] _colliders = new Collider[1]; // only ever return one ground collider
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float _rotation;
    private float _jumpVelocity;
    private Vector3 _horizontalVelocity;
    private Vector3 _verticalVelocity;
    private Collider _groundCollider;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected virtual void Awake() {
        // get character controller component (this will not be null)
        _characterController = GetComponent<CharacterController>();
        
        // initialise state machine
        StateMachine = new CharacterStateMachine();
    }

    protected virtual void Update() {
        // call update method on current character state
        StateMachine.CurrentCharacterState?.Update();
    }

    protected virtual void LateUpdate() {
        // add influencing factors to player movement
        AddPlatformMovement();
        AddPlatformRotation();
        AddGravity();
        
        // move character controller using motion and gravity
        // NB: gravity is the only vector multiplied by delta time
        _characterController.Move(_horizontalVelocity + _verticalVelocity * Time.deltaTime);
        
        // rotation character transform
        transform.Rotate(transform.up, _rotation);
        
        // reset horizontal, jump velocity and rotation fields
        _horizontalVelocity = Vector3.zero;
        _jumpVelocity = 0f;
        _rotation = 0f;
        
        // reset the ground collider
        _groundCollider = GetGroundCollider();
    }
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks (unused)
    //:::::::::::::::::::::::::::::://
    
    protected virtual void OnEnable() { }
    protected virtual void Start() { }
    protected virtual void OnDisable() { }
    
    //:::::::::::::::::::::::::::::://
    // Gizmo Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnDrawGizmosSelected() {
        // get character controller (this will never be null)
        _characterController = GetComponent<CharacterController>();
        
        // check grounded state (this requires CharacterController)
        _groundCollider = GetGroundCollider();
        
        // draw sphere gizmo
        Gizmos.color = IsGrounded ? new Color(0.0f, 1.0f, 0.0f, 0.35f) : new Color(1.0f, 0.0f, 0.0f, 0.35f);
        Gizmos.DrawSphere(GroundOverlapSpherePosition, GroundOverlapSphereRadius);
    }

    //:::::::::::::::::::::::::::::://
    // Adding Motion
    //:::::::::::::::::::::::::::::://

    protected void AddMotion(Vector3 motion) {
        // increment motion vector (ignoring y axis)
        _horizontalVelocity.x += motion.x;
        _horizontalVelocity.z += motion.z;
    }

    protected void AddJump(float velocity) {
        // increment jump velocity
        _jumpVelocity += velocity;
    }

    protected void AddRotation(float angle) {
        _rotation += angle;
    }
    
    //:::::::::::::::::::::::::::::://
    // Influencing Forces
    //:::::::::::::::::::::::::::::://

    private void AddPlatformMovement() {
        // if there is currently no ground collider; we're done
        if (!_groundCollider) return;
        
        // attempt to get MovingPlatform components linked to this collider
        var movingPlatforms = ComponentRegistry.ColliderComponents<MovingPlatform>(_groundCollider);
        
        // if MovingPlatforms returned; add motion (using only the first component)
        if (0 < movingPlatforms.Length) AddMotion(movingPlatforms[0].Motion);
    }
    
    private void AddPlatformRotation() {
        // if there is currently no ground collider; we're done
        if (!_groundCollider) return;
        
        // attempt to get RotatingPlatform components linked to this collider
        var rotatingPlatforms = ComponentRegistry.ColliderComponents<RotatingPlatform>(_groundCollider);
        
        // if no rotating platforms returned; we're done
        if (0 == rotatingPlatforms.Length) return;
        
        // get the first platform
        var rotatingPlatform = rotatingPlatforms[0];
        
        // calculate how far the platform will rotate this frame
        var rotationAngle = rotatingPlatform.RotationSpeed * Time.deltaTime;
        
        // calculate vectors
        // see https://stackoverflow.com/questions/79332117/how-do-i-calculate-a-vector-on-the-edge-of-a-circle-in-unity-for-a-charactercont
        var v1 = rotatingPlatform.transform.position;
        var v2 = transform.position;
        var v3 = Quaternion.Euler(0f, rotationAngle, 0f) * (v2 - v1) + v1;
        
        // move character on platform
        AddMotion(v3 - v2);
    }
    
    private void AddGravity() {
        // if character is grounded and vertical velocity is less than zero, reset vertical velocity (to jump velocity which may be zero)
        // we need to check y value because player may still be 'grounded' after the first frame(s) of a jump
        if (IsGrounded && _verticalVelocity.y < 0f) _verticalVelocity.y = _jumpVelocity;
        
        // factor in gravity
        _verticalVelocity.y += gravity * Time.deltaTime;
    }
    
    /*private void AddVoidVacuum() {
        // if the void is not active we're done
        if (!_void.IsActivated) return;

        // get character and void positions
        var characterPosition = transform.position;
        var voidPosition = _void.transform.position;

        // calculate the distance between character and void
        var distance = Vector3.Distance(characterPosition, voidPosition);

        // if the distance is outside the range we're done (no forces required)
        if (distance < _void.MinimumDistance || distance > _void.MaximumDistance) return;

        // calculate the normalized force inversely proportional to distance (as negative value)
        // this will make the force stronger is we get closer to the void
        var forceMagnitude = _void.MaximumForce * -(1f - Mathf.InverseLerp(_void.MinimumDistance, _void.MaximumDistance, distance));

        // get the direction vector from the character to the void
        var direction = (characterPosition - voidPosition).normalized;

        // add motion
        AddMotion(forceMagnitude * Time.deltaTime * direction);
    }*/
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private Collider GetGroundCollider() {
        // find the first ground object (if any) ignoring trigger colliders (this will return a maximum of one value)
        var count = Physics.OverlapSphereNonAlloc(GroundOverlapSpherePosition, GroundOverlapSphereRadius, _colliders, groundLayers, QueryTriggerInteraction.Ignore);
        
        // return ground collider
        return 0 < count ? _colliders[0] : null;
    }

    private Vector3 GetGroundOverlapSpherePosition() {
        // get the character position and add offset to y
        var position = _characterController.bounds.center;
        position.y = _characterController.bounds.min.y + _characterController.radius - k_GroundedOffset;
        
        // return the calculated position
        return position;
    }
}

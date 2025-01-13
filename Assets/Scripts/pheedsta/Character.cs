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
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Physics")]
    [Tooltip("The character uses its own gravity value (default is -9.81f)")]
    [SerializeField] protected float gravity = -9.81f;
    
    [Header("Ground")]
    [Tooltip("Increase this value for rough ground (should always be greater than skin width)")]
    [Range(0.1f, 1f)]
    [SerializeField] private float groundedOffset = 0.1f;
    [Tooltip("What layers the character uses as ground")]
    [SerializeField] private LayerMask groundLayers;
    
    //:::::::::::::::::::::::::::::://
    // Protected Properties
    //:::::::::::::::::::::::::::::://
    
    protected bool IsGrounded => GroundCollider;
    protected Collider GroundCollider { get; private set; }
    
    //:::::::::::::::::::::::::::::://
    // Private Properties
    //:::::::::::::::::::::::::::::://

    private float GroundOverlapSphereRadius => _characterController.radius;
    private Vector3 GroundOverlapSpherePosition => GetGroundOverlapSpherePosition();
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private CharacterController _characterController;
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly Collider[] _colliders = new Collider[1];
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private float _rotation;
    private float _jumpVelocity;
    private Vector3 _motion;
    private Vector3 _verticalVelocity;
    
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
        // call state update method
        StateMachine.CurrentCharacterState?.Update();
    }

    protected virtual void LateUpdate() {
        // add platform movement / rotation to the motion
        AddPlatformMovement();
        AddPlatformRotation();
        
        // add other influencing factors to the motion
        AddGravity();
        
        // move and rotate the character
        Move();
        Rotate();
        
        // reset the ground collider
        GroundCollider = GetGroundCollider();
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
        GroundCollider = GetGroundCollider();
        
        // draw sphere gizmo
        Gizmos.color = IsGrounded ? new Color(0.0f, 1.0f, 0.0f, 0.35f) : new Color(1.0f, 0.0f, 0.0f, 0.35f);
        Gizmos.DrawSphere(GroundOverlapSpherePosition, GroundOverlapSphereRadius);
    }

    //:::::::::::::::::::::::::::::://
    // Adding Motion
    //:::::::::::::::::::::::::::::://

    protected void AddMotion(Vector3 motion) {
        // increment motion vector (ignoring y axis)
        _motion.x += motion.x;
        _motion.z += motion.z;
    }

    protected void AddJump(float jumpVelocity) {
        // increment jump velocity (only if value passed is positive)
        if (0f < jumpVelocity) _jumpVelocity += jumpVelocity;
    }

    protected void AddRotation(float angle) {
        _rotation += angle;
    }
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private Collider GetGroundCollider() {
        // find the first ground object (if any) ignoring trigger colliders
        var count = Physics.OverlapSphereNonAlloc(GroundOverlapSpherePosition, GroundOverlapSphereRadius, _colliders, groundLayers, QueryTriggerInteraction.Ignore);
        
        // return ground collider
        return 0 < count ? _colliders[0] : null;
    }

    private Vector3 GetGroundOverlapSpherePosition() {
        // get the character position and add offset to y
        var position = _characterController.bounds.center;
        position.y = _characterController.bounds.min.y + _characterController.radius - groundedOffset;
        
        // return the calculated position
        return position;
    }
    
    //:::::::::::::::::::::::::::::://
    // Moving & Rotating
    //:::::::::::::::::::::::::::::://

    private void Move() {
        // move character controller using motion and gravity
        // NB: gravity is the only vector multiplied by delta time
        _characterController.Move(_motion + _verticalVelocity * Time.deltaTime);
        
        // reset motion field
        _motion = Vector3.zero;
    }

    private void Rotate() {
        // rotate character controller
        transform.Rotate(transform.up, _rotation);
        
        // reset rotation field
        _rotation = 0f;
    }
    
    //:::::::::::::::::::::::::::::://
    // Influencing Forces
    //:::::::::::::::::::::::::::::://

    private void AddPlatformMovement() {
        // if there is currently no ground collider; we're done
        if (!GroundCollider) return;
        
        // attempt to get MovingPlatform components linked to this collider
        var movingPlatforms = ComponentRegistry.ColliderComponents<MovingPlatform>(GroundCollider);
        
        // if MovingPlatforms returned; add motion
        if (0 < movingPlatforms.Length) AddMotion(movingPlatforms[0].Motion);
    }
    
    private void AddPlatformRotation() {
        // if there is currently no ground collider; we're done
        if (!GroundCollider) return;
        
        // attempt to get RotatingPlatform components linked to this collider
        var rotatingPlatforms = ComponentRegistry.ColliderComponents<RotatingPlatform>(GroundCollider);
        
        // if no rotating platforms returned; we're done
        if (0 == rotatingPlatforms.Length) return;
        
        // get the first platform
        var rotatingPlatform = rotatingPlatforms[0];
        
        // calculate how far the platform will rotate this frame
        var rotationAngle = rotatingPlatform.RotationSpeed * Time.deltaTime;
        
        // rotate character same as platform
        AddRotation(rotationAngle);
        
        // calculate vectors
        // see https://stackoverflow.com/questions/79332117/how-do-i-calculate-a-vector-on-the-edge-of-a-circle-in-unity-for-a-charactercont
        var v1 = rotatingPlatform.transform.position;
        var v2 = transform.position;
        var v3 = Quaternion.Euler(0f, rotationAngle, 0f) * (v2 - v1) + v1;
        
        // move character on platform
        AddMotion(v3 - v2);
    }
    
    private void AddGravity() {
        // if character is grounded AND they're not jumping, set vertical velocity to jump velocity (this will be zero if jump action not set)
        // we need to check y value because player may still be 'grounded' after the first frame of a jump
        if (IsGrounded && _verticalVelocity.y <= 0f) _verticalVelocity.y = _jumpVelocity;
        
        // reset jump field to default
        _jumpVelocity = 0f;
        
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
}

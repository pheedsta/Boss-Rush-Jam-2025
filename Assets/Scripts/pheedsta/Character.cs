using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Character
//++++++++++++++++++++++++++++++++++++++++//

public class Character : MonoBehaviour {
    
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
    // Properties
    //:::::::::::::::::::::::::::::://

    private bool IsGrounded => _groundCollider;
    private CharacterController Controller => GetCharacterController();
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private CharacterController _characterController;
    
    //:::::::::::::::::::::::::::::://
    // Managers
    //:::::::::::::::::::::::::::::://

    private Void _void;
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly Collider[] _colliders = new Collider[1];
    
    //:::::::::::::::::::::::::::::://
    // Gizmo Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly Color _gizmoGroundedColor = new (0.0f, 1.0f, 0.0f, 0.35f);
    private readonly Color _gizmoUngroundedColor = new (1.0f, 0.0f, 0.0f, 0.35f);
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private float _rotation;
    private float _jumpVelocity;
    private Vector3 _motion;
    private Vector3 _verticalVelocity;
    private Collider _groundCollider;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected virtual void Awake() {
        // get managers
        _void = Void.Instance;
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_void, "Void is missing");
        //++++++++++++++++++++++++++++++++++++++++//
    }

    protected virtual void Update() {
        // get the current ground collider (if any)
        GetGroundCollider();
        
        // add platform movement / rotation to the motion
        AddPlatformMovement();
        AddPlatformRotation();
        
        // add other influencing factors to the motion
        AddVoidVacuum();
        AddGravity();
        
        // move and rotate the character
        Move();
        Rotate();
    }
    
    //:::::::::::::::::::::::::::::://
    // Gizmo Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnDrawGizmosSelected() {
        // update gizmo color depending on grounded status
        Gizmos.color = IsGrounded ? _gizmoGroundedColor : _gizmoUngroundedColor;
        
        // calculate the sphere position (should be the same as Physics.OverlapSphereNonAlloc in GetGroundCollider)
        var position = transform.position;
        position.y += Controller.radius - groundedOffset;
        
        // draw gizmo sphere
        Gizmos.DrawSphere(position, Controller.radius);
    }
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private CharacterController GetCharacterController() {
        // if a character controller has already been set, we're done
        if (_characterController) return _characterController;
        
        // get character controller component
        _characterController = GetComponent<CharacterController>();
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_characterController, "CharacterController Component is missing");
        //++++++++++++++++++++++++++++++++++++++++//
        
        // return character controller
        return _characterController;
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
    // Proximity Methods
    //:::::::::::::::::::::::::::::://

    private void GetGroundCollider() {
        // get the character position and add offset to y
        var position = transform.position;
        position.y += Controller.radius - groundedOffset;
        
        // starting find the first ground object (if any) and ignore trigger colliders
        var count = Physics.OverlapSphereNonAlloc(position, Controller.radius, _colliders, groundLayers, QueryTriggerInteraction.Ignore);
        
        // update ground collider
        _groundCollider = 0 < count ? _colliders[0] : null;
    }
    
    //:::::::::::::::::::::::::::::://
    // Moving & Rotating
    //:::::::::::::::::::::::::::::://

    private void Move() {
        // move character controller using motion and gravity
        // NB: gravity is the only vector multiplied by delta time
        Controller.Move(_motion + _verticalVelocity * Time.deltaTime);
        
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
        if (!_groundCollider) return;
        
        // attempt to get the collider parent from the collider registry
        var colliderParent = ColliderRegistry.GetColliderParent(_groundCollider);
        
        // if the collider parent is a MovingPlatform, add motion
        if (colliderParent && colliderParent is MovingPlatform movingPlatform) AddMotion(movingPlatform.Motion);
    }
    
    private void AddPlatformRotation() {
        // if there is currently no ground collider; we're done
        if (!_groundCollider) return;
        
        // attempt to get the collider parent from the collider registry
        var colliderParent = ColliderRegistry.GetColliderParent(_groundCollider);
        
        // if RotatingPlatform wasn't a parent of the collider we're done
        if (!colliderParent || colliderParent is not RotatingPlatform rotatingPlatform) return;
        
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

    private void AddVoidVacuum() {
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
}

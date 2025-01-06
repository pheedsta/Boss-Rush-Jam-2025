using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Character
//++++++++++++++++++++++++++++++++++++++++//

public class Character : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://
    
    protected bool IsGrounded => _characterController && _characterController.isGrounded;
    
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
    
    private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private int _platformLayerMask;
    private float _jumpVelocity;
    private Vector3 _motion;
    private Vector3 _gravity;
    private Component _floorComponent;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected virtual void Awake() {
        // get Components
        _characterController = GetComponent<CharacterController>();
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_characterController, "CharacterController Component is missing");
        //++++++++++++++++++++++++++++++++++++++++//
        
        // get managers
        _void = Void.Instance;
        //++++++++++++++++++++++++++++++++++++++++//
        Debug.Assert(_void, "Void is missing");
        //++++++++++++++++++++++++++++++++++++++++//

        // get 'Platform' layer mask
        _platformLayerMask = LayerMask.GetMask("Platform");
    }

    protected virtual void Update() {
        // reset motion and jump values
        _motion = Vector3.zero;
        _jumpVelocity = 0f;
    }

    protected virtual void LateUpdate() {
        // attempt to get ground object underneath the character
        var ground = GetGround();
        
        // add platform movement / rotation to the motion
        AddPlatformMovement(ground);
        AddPlatformRotation(ground);
        
        // add other influencing factors to the motion
        AddVoidVacuum();
        AddGravity();
        
        // move character controller using motion and gravity
        // NB: gravity is the only vector multiplied by delta time
        _characterController.Move(_motion + _gravity * Time.deltaTime);
    }

    //:::::::::::::::::::::::::::::://
    // Adding Motion
    //:::::::::::::::::::::::::::::://

    protected void AddMotion(Vector3 motion) {
        // increment motion vector (ignoring y axis)
        _motion.x += motion.x;
        _motion.z += motion.z;
    }
    
    //:::::::::::::::::::::::::::::://
    // Proximity Methods
    //:::::::::::::::::::::::::::::://

    private Component GetGround() {
        // if character isn't grounded we're done
        if (!IsGrounded) return null;
        
        // get the character controller bounds and ray length
        var bounds = _characterController.bounds;
        var maxDistance = bounds.extents.y + _characterController.skinWidth * 2f; // add skin width twice to allow for minor variations
        
        // raycast down to ground (just below character controller)
        var count = Physics.RaycastNonAlloc(bounds.center, Vector3.down, _raycastHits, maxDistance, _platformLayerMask);
        
        // return the parent of the first collider from ColliderRegistry (if any) 
        return count == 0 ? null : ColliderRegistry.GetColliderParent(_raycastHits[0].collider);
    }
    
    //:::::::::::::::::::::::::::::://
    // Influencing Forces
    //:::::::::::::::::::::::::::::://

    private void AddPlatformMovement(Component ground) {
        if (ground && ground is MovingPlatform movingPlatform) AddMotion(movingPlatform.Motion);
    }
    
    private void AddPlatformRotation(Component ground) {
        // if RotatingPlatform wasn't passed we're done
        if (!ground || ground is not RotatingPlatform rotatingPlatform) return;
        
        // calculate how far the platform will rotate this frame
        var rotationAngle = rotatingPlatform.RotationSpeed * Time.deltaTime;
        
        // rotate character
        transform.Rotate(transform.up, rotationAngle);
        
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
        if (0f < _jumpVelocity) {
            // character is jumping, update gravity with jump velocity
            _gravity.y = _jumpVelocity;
        } else if (IsGrounded) {
            // character is grounded, reset gravity velocity
            _gravity.y = 0f;
        }
        
        // add gravity (downwards)
        _gravity.y += Constant.Value.Gravity * Time.deltaTime;
    }

    protected void AddJump(float jumpVelocity) {
        // increment jump velocity (only if value passed is positive)
        if (0f < jumpVelocity) _jumpVelocity += jumpVelocity;
    }
}

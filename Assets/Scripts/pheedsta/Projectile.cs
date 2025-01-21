using UnityEngine;

//------------------------------//
// Required Components
//------------------------------//

[RequireComponent(typeof(Rigidbody))]

//++++++++++++++++++++++++++++++//
// CLASS: Projectile
//++++++++++++++++++++++++++++++//

public class Projectile : MonoBehaviour, IReusable {
    
    //------------------------------//
    // IReusable Properties
    //------------------------------//

    public string Identifier => identifier;
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("IReusable")]
    [SerializeField] private string identifier;

    [Header("Physics")]
    [SerializeField] private float force = 10f;
    [SerializeField] private float torque = 10f;
    [SerializeField] private float floorY = -20f;
    
    [Header("Prefabs")]
    [SerializeField] private Explosion explosionPrefab;
    
    [Header("Sound")]
    [Tooltip("The SFX that will play when projectile starts")]
    [SerializeField] private AK.Wwise.Event startSound;
    [Tooltip("The SFX that will play when projectile hits and explodes")]
    [SerializeField] private AK.Wwise.Event hitSound;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://

    private Rigidbody _rigidbody;
    private Transform _cubeTransform;
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private bool _addForce;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Awake() {
        Configure();
    }

    private void OnEnable() {
        // update flag so we add Impulse for during next FixedUpdate
        _addForce = true;
        
        // play start sound
        startSound.Post(gameObject);
    }

    private void Update() {
        // if fire ability is below the floor threshold, destroy it
        if (transform.position.y < floorY) ReusablePool.ReturnReusable(this);
    }

    private void FixedUpdate() {
        // if we need to add the initial Impulse, do so
        if (_addForce) _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        
        // reset flag
        _addForce = false;

        // add torque to rigid body so it rotates as well
        _rigidbody.AddTorque(Vector3.up * torque, ForceMode.Force);
    }

    private void OnDisable() {
        // reset rigid body so no forces are acting on it next time it's used
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    
    //:::::::::::::::::::::::::::::://
    // Collision Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnCollisionEnter(Collision other) {
        // play hit sound
        hitSound.Post(gameObject);

        // if explosion prefab is set, instantiate it at the current position
        if (explosionPrefab) _ = ReusablePool.FetchReusable(explosionPrefab, transform.position, transform.rotation);
        
        // return projectile to prefab pool
        ReusablePool.ReturnReusable(this);
    }

    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // get required components (these will not be null)
        _rigidbody = GetComponent<Rigidbody>();
    }
}

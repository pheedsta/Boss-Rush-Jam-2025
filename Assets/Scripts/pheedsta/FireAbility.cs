using System;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: FireAbility
//++++++++++++++++++++++++++++++//

public class FireAbility : MonoBehaviour, IReusable {
    
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
    [SerializeField] private LayerMask collisionMask;
    
    
    [Header("Sound")]
    [Tooltip("The SFX that will play when fire ball starts")]
    [SerializeField] private AK.Wwise.Event fireStartSound;
    [Tooltip("The SFX that will play when fire ball hits and explodes")]
    [SerializeField] private AK.Wwise.Event fireHitSound;
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://
    
    private const float k_MoveSpeed = 30f;
    private const float k_RotationSpeed = 200f;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://

    private Transform _cubeTransform;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Awake() {
        Configure();
    }

    private void OnEnable() {
        // play fire start sound
        fireStartSound.Post(gameObject);
    }

    private void Update() {
        // rotate cube
        _cubeTransform.Rotate(_cubeTransform.up, k_RotationSpeed * Time.deltaTime);
        
        // move fire ability
        transform.Translate(k_MoveSpeed * Time.deltaTime * Vector3.forward);

        // if we haven't hit anything; we're done
        if (!Physics.CheckSphere(transform.position, 0.5f, collisionMask, QueryTriggerInteraction.Ignore)) return;
        
        // fire ball has hit something; trigger sound and return it to the reusable pool
        fireHitSound.Post(gameObject);
        ReusablePool.ReturnReusable(this);
    }

    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        _cubeTransform = transform.Find("Cube");
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_cubeTransform, "'Cube' transform is missing");
        //++++++++++++++++++++++++++++++//
    }
}

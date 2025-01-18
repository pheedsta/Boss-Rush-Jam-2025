using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Shard
//++++++++++++++++++++++++++++++++++++++++//

public class Shard : MonoBehaviour, IReusable {
    
    //------------------------------//
    // IReusable Properties
    //------------------------------//

    public string Identifier => identifier;
    
    //------------------------------//
    // Properties
    //------------------------------//
    
    public float CollectDistance => collectDistance;
    public float VacuumDistance => vacuumDistance;
    public float VacuumSpeed => vacuumSpeed;
    
    //------------------------------//
    // Public Fields
    //------------------------------//
    
    [HideInInspector] public bool isMovingTowardsPlayer;
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("IReusable")]
    [SerializeField] private string identifier;
    
    [Header("Collection")]
    [Tooltip("The minimum distance shard needs to be from player before it is collected")]
    [SerializeField] private float collectDistance = 0.5f;
    [Tooltip("The minimum distance shard needs to be from player before it starts moving towards player")]
    [SerializeField] private float vacuumDistance = 4f;
    [Tooltip("The speed shard will move towards player")]
    [SerializeField] private float vacuumSpeed = 20f;
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://
    
    private const float k_RotationSpeed = 100f;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Update() {
        transform.Rotate(transform.up, k_RotationSpeed * Time.deltaTime);
    }

    private void OnDisable() {
        // reset fields to defaults
        isMovingTowardsPlayer = false;
    }
}

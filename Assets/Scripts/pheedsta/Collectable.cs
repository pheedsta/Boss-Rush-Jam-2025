using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Shard
//++++++++++++++++++++++++++++++++++++++++//

public class Collectable : MonoBehaviour, IReusable {
    
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
    [Tooltip("The minimum distance collectable needs to be from player before it is collected")]
    [SerializeField] private float collectDistance = 0.5f;
    [Tooltip("The minimum distance collectable needs to be from player before it starts moving towards player")]
    [SerializeField] private float vacuumDistance = 4f;
    [Tooltip("The speed collectable will move towards player")]
    [SerializeField] private float vacuumSpeed = 20f;
    
    [Header("Sound")]
    [Tooltip("The SFX that will play when collectable is collected")]
    public AK.Wwise.Event collectSound;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnDisable() {
        isMovingTowardsPlayer = false;
    }
}
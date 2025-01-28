using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: CollectableHeart
//++++++++++++++++++++++++++++++//

public class CollectableHeart : Collectable {
    
    //------------------------------//
    // Properties
    //------------------------------//

    public int Health => health;
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://
    
    private const float k_RotationSpeed = 200f;
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [SerializeField] private int health = 25;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Update() {
        transform.Rotate(transform.up, k_RotationSpeed * Time.deltaTime);
    }
}
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: CollectableShard
//++++++++++++++++++++++++++++++++++++++++//

public class CollectableShard : Collectable {
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://
    
    private const float k_RotationSpeed = 200f;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Update() {
        transform.Rotate(transform.up, k_RotationSpeed * Time.deltaTime);
    }
}

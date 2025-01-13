using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: RotatingPlatform
//++++++++++++++++++++++++++++++++++++++++//

public class RotatingPlatform : MonoBehaviour {
    
    //------------------------------//
    // Properties
    //------------------------------//

    public float RotationSpeed => rotationSpeed;
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://

    [SerializeField] private float rotationSpeed = 30f;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnEnable() {
        ComponentRegistry.Register(this);
    }

    private void Update() {
        transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
    }

    private void OnDisable() {
        ComponentRegistry.Deregister(this);
    }
}

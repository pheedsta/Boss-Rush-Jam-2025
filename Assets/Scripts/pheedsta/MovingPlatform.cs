using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: MovingPlatform
//++++++++++++++++++++++++++++++++++++++++//

public class MovingPlatform : MonoBehaviour {
    
    //------------------------------//
    // Properties
    //------------------------------//

    public Vector3 Motion { get; private set; }
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnEnable() {
        ColliderRegistry.Register(this);
    }

    private void Update() {
        // calculate motion vector and update property
        Motion = 3f * Time.deltaTime * transform.forward;
        
        // move platform
        transform.Translate(Motion, Space.World);
    }

    private void OnDisable() {
        ColliderRegistry.Deregister(this);
    }
}
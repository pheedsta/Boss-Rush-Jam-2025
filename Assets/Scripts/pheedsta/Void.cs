using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: Void
//++++++++++++++++++++++++++++++//

[DefaultExecutionOrder(-1)]
public class Void : MonoBehaviour {
    
    //------------------------------//
    // Static Properties
    //------------------------------//

    public static Void Instance { get; private set; }
    
    //------------------------------//
    // Properties
    //------------------------------//

    public bool IsActivated => isActivated;
    public float MaximumForce => maximumForce;
    public float MinimumDistance => minimumDistance;
    public float MaximumDistance => maximumDistance;
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://

    [Header("Force")] 
    [SerializeField] private bool isActivated = true;
    [SerializeField] private float maximumForce = 12f;
    [SerializeField] private float minimumDistance = 2f;
    [SerializeField] private float maximumDistance = 50f;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://
    
    private void Awake() {
        if (!Instance) {
            // an instance has not been set; update field
            Instance = this;
        } else {
            // an instance has already been set; destroy Component
            Destroy(this);
        }
    }
}

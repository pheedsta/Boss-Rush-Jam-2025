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

    private void Update() {
        // rotate cube
        _cubeTransform.Rotate(_cubeTransform.up, k_RotationSpeed * Time.deltaTime);
        
        // move fire ability
        transform.Translate(k_MoveSpeed * Time.deltaTime * Vector3.forward);
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

using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: InputManager
//++++++++++++++++++++++++++++++//

public class InputManager : MonoBehaviour {

    //------------------------------//
    // Static Properties
    //------------------------------//

    public static InputManager Instance;
    
    //:::::::::::::::::::::::::::::://
    // Private Properties
    //:::::::::::::::::::::::::::::://

    public Vector2 LookDelta => GetLookDelta();
    public Vector2 MoveDelta => GetMoveDelta();

    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private InputActions.PlayerActions _playerActions;

    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Awake() {
        if (Instance && Instance != this) {
            // a singleton has already been created; destroy this component
            Destroy(this);
        } else {
            // a singleton has not been created; set instance and configure
            Instance = this;
            Configure();
        }
    }

    private void OnEnable() {
        _playerActions.Enable();
    }

    private void OnDisable() {
        _playerActions.Disable();
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        _playerActions = new InputActions().Player;
    }
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private Vector2 GetLookDelta() {
        return _playerActions.Look.ReadValue<Vector2>();
    }
    
    private Vector2 GetMoveDelta() {
        return _playerActions.Move.ReadValue<Vector2>();
    }
}
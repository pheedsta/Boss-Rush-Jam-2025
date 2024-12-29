using UnityEngine;
using UnityEngine.InputSystem;

//++++++++++++++++++++++++++++++//
// CLASS: InputManager
//++++++++++++++++++++++++++++++//

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour {

    //------------------------------//
    // Static Properties
    //------------------------------//

    public static InputManager Instance;
    
    //------------------------------//
    // Delegates
    //------------------------------//

    public delegate void InputEvent();

    //------------------------------//
    // Events
    //------------------------------//

    public static event InputEvent OnJump;
    
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
        // subscribe (JUMP)
        //_playerActions.Jump.started += PlayerActions_Jump_Started;
        _playerActions.Jump.performed += PlayerActions_Jump_Performed;
        //_playerActions.Jump.canceled += PlayerActions_Jump_Canceled;

        // enable inputs
        _playerActions.Enable();
    }

    private void OnDisable() {
        // disable inputs
        _playerActions.Disable();
        
        // unsubscribe (JUMP)
        //_playerActions.Jump.started -= PlayerActions_Jump_Started;
        _playerActions.Jump.performed -= PlayerActions_Jump_Performed;
        //_playerActions.Jump.canceled -= PlayerActions_Jump_Canceled;
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // initialise player input actions
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

    //:::::::::::::::::::::::::::::://
    // PlayerAction Events
    //:::::::::::::::::::::::::::::://

    //private void PlayerActions_Jump_Started(InputAction.CallbackContext callbackContext) {
    //}

    private void PlayerActions_Jump_Performed(InputAction.CallbackContext callbackContext) {
        OnJump?.Invoke();
    }

    //private void PlayerActions_Jump_Canceled(InputAction.CallbackContext callbackContext) {
    //}
}
using UnityEngine;
using UnityEngine.InputSystem;

//++++++++++++++++++++++++++++++//
// CLASS: InputManager
//++++++++++++++++++++++++++++++//

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour {
    
    //------------------------------//
    // Enums
    //------------------------------//

    public enum ActionPhase {
        Started, Performed, Canceled
    }
    
    //------------------------------//
    // Delegates
    //------------------------------//

    public delegate void ValueEvent(ActionPhase phase, Vector2 value);
    public delegate void ButtonEvent(ActionPhase phase);

    //------------------------------//
    // Static Value Events
    //------------------------------//
    
    public static event ValueEvent OnMove = delegate { };
    public static event ValueEvent OnLook = delegate { };

    //------------------------------//
    // Static Button Events
    //------------------------------//
    
    public static event ButtonEvent OnJump = delegate { };

    //:::::::::::::::::::::::::::::://
    // Static Fields
    //:::::::::::::::::::::::::::::://

    private static InputManager _instance;

    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://
    
    private InputActions.PlayerActions _playerActions;

    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://
    
    private void Awake() {
        if (!_instance) {
            // an instance has not been set; update field and configure
            _instance = this;
            Configure();
        } else {
            // an instance has already been set; destroy Component
            Destroy(this);
        }
    }

    private void OnEnable() {
        // subscribe (Move)
        _playerActions.Move.started += PlayerActions_Move_Started;
        _playerActions.Move.performed += PlayerActions_Move_Performed;
        _playerActions.Move.canceled += PlayerActions_Move_Canceled;
        
        // subscribe (Look)
        _playerActions.Look.started += PlayerActions_Look_Started;
        _playerActions.Look.performed += PlayerActions_Look_Performed;
        _playerActions.Look.canceled += PlayerActions_Look_Canceled;
        
        // subscribe (JUMP)
        _playerActions.Jump.performed += PlayerActions_Jump_Performed;
        _playerActions.Jump.canceled += PlayerActions_Jump_Canceled;

        // enable inputs
        _playerActions.Enable();
    }

    private void OnDisable() {
        // disable inputs
        _playerActions.Disable();
        
        // unsubscribe (Move)
        _playerActions.Move.started -= PlayerActions_Move_Started;
        _playerActions.Move.performed -= PlayerActions_Move_Performed;
        _playerActions.Move.canceled -= PlayerActions_Move_Canceled;
        
        // unsubscribe (Look)
        _playerActions.Look.started -= PlayerActions_Look_Started;
        _playerActions.Look.performed -= PlayerActions_Look_Performed;
        _playerActions.Look.canceled -= PlayerActions_Look_Canceled;
        
        // unsubscribe (JUMP)
        _playerActions.Jump.performed -= PlayerActions_Jump_Performed;
        _playerActions.Jump.canceled -= PlayerActions_Jump_Canceled;
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // initialise input actions
        _playerActions = new InputActions().Player;
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Move Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Move_Started(InputAction.CallbackContext callbackContext) {
        OnMove.Invoke(ActionPhase.Started, callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Move_Performed(InputAction.CallbackContext callbackContext) {
        OnMove.Invoke(ActionPhase.Performed, callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Move_Canceled(InputAction.CallbackContext callbackContext) {
        OnMove.Invoke(ActionPhase.Canceled, callbackContext.ReadValue<Vector2>());
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Look Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Look_Started(InputAction.CallbackContext callbackContext) {
        OnLook.Invoke(ActionPhase.Started, callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Look_Performed(InputAction.CallbackContext callbackContext) {
        OnLook.Invoke(ActionPhase.Performed, callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Look_Canceled(InputAction.CallbackContext callbackContext) {
        OnLook.Invoke(ActionPhase.Canceled, callbackContext.ReadValue<Vector2>());
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Jump Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Jump_Performed(InputAction.CallbackContext callbackContext) {
        OnJump.Invoke(ActionPhase.Performed);
    }

    private static void PlayerActions_Jump_Canceled(InputAction.CallbackContext callbackContext) {
        OnJump.Invoke(ActionPhase.Canceled);
    }
}
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
    public static event ButtonEvent OnSprint = delegate { };

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
        
        // subscribe (SPRINT)
        _playerActions.Sprint.performed += PlayerActions_Sprint_Performed;
        _playerActions.Sprint.canceled += PlayerActions_Sprint_Canceled;

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
        
        // unsubscribe (SPRINT)
        _playerActions.Sprint.performed -= PlayerActions_Sprint_Performed;
        _playerActions.Sprint.canceled -= PlayerActions_Sprint_Canceled;
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // initialise input actions
        _playerActions = new InputActions().Player;
    }
    
    //:::::::::::::::::::::::::::::://
    // Keyboard Input
    //:::::::::::::::::::::::::::::://

    private static Vector2 DigitiseKeyboardInput(Vector2 input) {
        // initialise field
        var digitisedInput = Vector2.zero;
        
        // digitise X value
        digitisedInput.x = input.x switch {
            < 0f => -1f,
            > 0f => 1f,
            _ => 0f
        };
        
        // digitise Y value
        digitisedInput.y = input.y switch {
            < 0f => -1f,
            > 0f => 1f,
            _ => 0f
        };

        // return newly digitised vector
        return digitisedInput;
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Move Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Move_Started(InputAction.CallbackContext callbackContext) {
        // get input and digitise it if it was a keyboard
        var input = callbackContext.ReadValue<Vector2>();
        if (callbackContext.control.device == Keyboard.current) input = DigitiseKeyboardInput(input);
        
        // invoke event
        OnMove.Invoke(ActionPhase.Started, input);
    }

    private static void PlayerActions_Move_Performed(InputAction.CallbackContext callbackContext) {
        // get input and digitise it if it was a keyboard
        var input = callbackContext.ReadValue<Vector2>();
        if (callbackContext.control.device == Keyboard.current) input = DigitiseKeyboardInput(input);
        
        // invoke event
        OnMove.Invoke(ActionPhase.Performed, input);
    }

    private static void PlayerActions_Move_Canceled(InputAction.CallbackContext callbackContext) {
        // get input and digitise it if it was a keyboard
        var input = callbackContext.ReadValue<Vector2>();
        if (callbackContext.control.device == Keyboard.current) input = DigitiseKeyboardInput(input);
        
        // invoke event
        OnMove.Invoke(ActionPhase.Canceled, input);
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

    //:::::::::::::::::::::::::::::://
    // PlayerAction Sprint Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Sprint_Performed(InputAction.CallbackContext callbackContext) {
        OnSprint.Invoke(ActionPhase.Performed);
    }

    private static void PlayerActions_Sprint_Canceled(InputAction.CallbackContext callbackContext) {
        OnSprint.Invoke(ActionPhase.Canceled);
    }
}
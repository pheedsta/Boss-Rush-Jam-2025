using UnityEngine;
using UnityEngine.InputSystem;

//++++++++++++++++++++++++++++++//
// CLASS: InputManager
//++++++++++++++++++++++++++++++//

[DefaultExecutionOrder(-2)]
public class InputManager : MonoBehaviour {
    
    //------------------------------//
    // Enums
    //------------------------------//

    public enum ActionPhase {
        Started, Performed, Canceled
    }

    public enum InputDevice {
        Keyboard, Mouse, Gamepad
    }
    
    //------------------------------//
    // Delegates
    //------------------------------//

    public delegate void ValueEvent(ActionPhase phase, InputDevice inputDevice, Vector2 value);
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
    public static event ButtonEvent OnDash = delegate { };
    public static event ButtonEvent OnAttack = delegate { };
    public static event ButtonEvent OnSpecial = delegate { };

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
        // subscribe (MOVE)
        _playerActions.Move.started += PlayerActions_Move_Started;
        _playerActions.Move.performed += PlayerActions_Move_Performed;
        _playerActions.Move.canceled += PlayerActions_Move_Canceled;
        
        // subscribe (LOOK)
        _playerActions.Look.started += PlayerActions_Look_Started;
        _playerActions.Look.performed += PlayerActions_Look_Performed;
        _playerActions.Look.canceled += PlayerActions_Look_Canceled;
        
        // subscribe (JUMP)
        _playerActions.Jump.performed += PlayerActions_Jump_Performed;
        _playerActions.Jump.canceled += PlayerActions_Jump_Canceled;
        
        // subscribe (DASH)
        _playerActions.Dash.performed += PlayerActions_Dash_Performed;
        _playerActions.Dash.canceled += PlayerActions_Dash_Canceled;
        
        // subscribe (ATTACK)
        _playerActions.Attack.performed += PlayerActions_Attack_Performed;
        _playerActions.Attack.canceled += PlayerActions_Attack_Canceled;
        
        // subscribe (SPECIAL)
        _playerActions.Special.performed += PlayerActions_Special_Performed;
        _playerActions.Special.canceled += PlayerActions_Special_Canceled;

        // enable inputs
        _playerActions.Enable();
    }

    private void OnDisable() {
        // disable inputs
        _playerActions.Disable();
        
        // unsubscribe (MOVE)
        _playerActions.Move.started -= PlayerActions_Move_Started;
        _playerActions.Move.performed -= PlayerActions_Move_Performed;
        _playerActions.Move.canceled -= PlayerActions_Move_Canceled;
        
        // unsubscribe (LOOK)
        _playerActions.Look.started -= PlayerActions_Look_Started;
        _playerActions.Look.performed -= PlayerActions_Look_Performed;
        _playerActions.Look.canceled -= PlayerActions_Look_Canceled;
        
        // subscribe (JUMP)
        _playerActions.Jump.performed -= PlayerActions_Jump_Performed;
        _playerActions.Jump.canceled -= PlayerActions_Jump_Canceled;
        
        // subscribe (DASH)
        _playerActions.Dash.performed -= PlayerActions_Dash_Performed;
        _playerActions.Dash.canceled -= PlayerActions_Dash_Canceled;
        
        // subscribe (ATTACK)
        _playerActions.Attack.performed -= PlayerActions_Attack_Performed;
        _playerActions.Attack.canceled -= PlayerActions_Attack_Canceled;
        
        // subscribe (SPECIAL)
        _playerActions.Special.performed -= PlayerActions_Special_Performed;
        _playerActions.Special.canceled -= PlayerActions_Special_Canceled;
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

    private static InputDevice GetInputDevice(InputAction.CallbackContext callbackContext) {
        if (callbackContext.control.device == Keyboard.current) return InputDevice.Keyboard;
        return callbackContext.control.device == Mouse.current ? InputDevice.Mouse : InputDevice.Gamepad;
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Move Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Move_Started(InputAction.CallbackContext callbackContext) {
        OnMove.Invoke(ActionPhase.Started, GetInputDevice(callbackContext), callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Move_Performed(InputAction.CallbackContext callbackContext) {
        OnMove.Invoke(ActionPhase.Performed, GetInputDevice(callbackContext), callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Move_Canceled(InputAction.CallbackContext callbackContext) {
        OnMove.Invoke(ActionPhase.Canceled, GetInputDevice(callbackContext), callbackContext.ReadValue<Vector2>());
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Look Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Look_Started(InputAction.CallbackContext callbackContext) {
        OnLook.Invoke(ActionPhase.Started, GetInputDevice(callbackContext), callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Look_Performed(InputAction.CallbackContext callbackContext) {
        OnLook.Invoke(ActionPhase.Performed, GetInputDevice(callbackContext), callbackContext.ReadValue<Vector2>());
    }

    private static void PlayerActions_Look_Canceled(InputAction.CallbackContext callbackContext) {
        OnLook.Invoke(ActionPhase.Canceled, GetInputDevice(callbackContext), callbackContext.ReadValue<Vector2>());
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
    // PlayerAction Dash Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Dash_Performed(InputAction.CallbackContext callbackContext) {
        OnDash.Invoke(ActionPhase.Performed);
    }

    private static void PlayerActions_Dash_Canceled(InputAction.CallbackContext callbackContext) {
        OnDash.Invoke(ActionPhase.Canceled);
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Attack Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Attack_Performed(InputAction.CallbackContext callbackContext) {
        OnAttack.Invoke(ActionPhase.Performed);
    }

    private static void PlayerActions_Attack_Canceled(InputAction.CallbackContext callbackContext) {
        OnAttack.Invoke(ActionPhase.Canceled);
    }

    //:::::::::::::::::::::::::::::://
    // PlayerAction Special Events
    //:::::::::::::::::::::::::::::://

    private static void PlayerActions_Special_Performed(InputAction.CallbackContext callbackContext) {
        OnSpecial.Invoke(ActionPhase.Performed);
    }

    private static void PlayerActions_Special_Canceled(InputAction.CallbackContext callbackContext) {
        OnSpecial.Invoke(ActionPhase.Canceled);
    }
}
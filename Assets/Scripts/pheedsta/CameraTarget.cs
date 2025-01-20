using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: Player
//++++++++++++++++++++++++++++++++++++++++//

public class CameraTarget : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Turning & Aiming")]
    [Tooltip("The speed at which camera rotates left and right")]
    [SerializeField] private float yawSpeed = 50f;
    [Tooltip("The speed at which camera rotates up and down")]
    [SerializeField] private float pitchSpeed = 100f;
    [Range(-10f, -90f)]
    [Tooltip("How far in degrees the aim camera rotates down")]
    [SerializeField] private float minimumPitch = -40.0f;
    [Range(10f, 90f)]
    [Tooltip("How far in degrees the aim camera rotates up")]
    [SerializeField] private float maximumPitch = 60.0f;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private Vector2 _lookDelta;
    private Vector3 _cameraRotation;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnEnable() {
        InputManager.OnLook += InputManager_OnLook;
    }

    private void Update() {
        Look();
    }

    private void OnDisable() {
        InputManager.OnLook -= InputManager_OnLook;
    }
    
    //:::::::::::::::::::::::::::::://
    // Look
    //:::::::::::::::::::::::::::::://

    private void Look() {
        // calculate camera yaw (rotation left and right)
        _cameraRotation.y += _lookDelta.x * yawSpeed * Time.deltaTime;
        
        // calculate camera pitch (rotation up and down) clamping to min / max
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x + _lookDelta.y * pitchSpeed * Time.deltaTime, minimumPitch, maximumPitch);
    
        // rotate camera using yaw and pitch
        transform.localEulerAngles = _cameraRotation;
    }
    
    //:::::::::::::::::::::::::::::://
    // InputManager Events
    //:::::::::::::::::::::::::::::://

    private void InputManager_OnLook(InputManager.ActionPhase phase, InputManager.InputDevice inputDevice, Vector2 value) {
        _lookDelta = value;
    }
}
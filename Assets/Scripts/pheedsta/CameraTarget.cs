using System.Collections;
using Cinemachine;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: Player
//++++++++++++++++++++++++++++++//

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
    
    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [Tooltip("How close the camera is to the player when they are aiming")]
    [SerializeField] private float aimingCameraDistance;
    [Tooltip("The shoulder offset when player is aiming")]
    [SerializeField] private Vector3 aimingShoulderOffset;
    [Tooltip("The time it takes to transition to and from aiming settings")]
    [SerializeField] private float transitionDuration = 0.3f;
    [Tooltip("The time it takes to before camera returns to default settings after aiming")]
    [SerializeField] private float pauseDuration = 0.3f;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private Vector2 _lookDelta;
    private Vector3 _cameraRotation;
    private Cinemachine3rdPersonFollow _cinemachineThirdPersonFollow;
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float progress;
    private float _cameraDistance;
    private Vector3 _shoulderOffset;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Awake() {
        //++++++++++++++++++++++++++++++//
        Debug.Assert(virtualCamera, "Cinemachine Virtual Camera is missing");
        //++++++++++++++++++++++++++++++//
    
        // get third person follow component so we can change camera position when aiming
        _cinemachineThirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_cinemachineThirdPersonFollow, "Cinemachine3rdPersonFollow component is missing");
        //++++++++++++++++++++++++++++++//
        
        // get default parameters
        _cameraDistance = _cinemachineThirdPersonFollow.CameraDistance;
        _shoulderOffset = _cinemachineThirdPersonFollow.ShoulderOffset;
    }

    private void OnEnable() {
        InputManager.OnLook += InputManager_OnLook;
    }

    private void Update() {
        Look();
    }

    private void OnDisable() {
        InputManager.OnLook -= InputManager_OnLook;
    }
    
    //------------------------------//
    // Aiming
    //------------------------------//

    public void StartAiming() {
        StopAllCoroutines();
        StartCoroutine(StartAimingCoroutine());
    }

    public void StopAiming() {
        StopAllCoroutines();
        StartCoroutine(StopAimingCoroutine());
    }
    
    //:::::::::::::::::::::::::::::://
    // Look
    //:::::::::::::::::::::::::::::://

    private void Look() {
        // calculate camera yaw (rotation left and right)
        _cameraRotation.y += _lookDelta.x * yawSpeed * Time.deltaTime;
        
        // calculate camera pitch (rotation up and down) clamping to min / max
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x - _lookDelta.y * pitchSpeed * Time.deltaTime, minimumPitch, maximumPitch);
    
        // rotate camera using yaw and pitch
        transform.localEulerAngles = _cameraRotation;
    }
    
    //:::::::::::::::::::::::::::::://
    // InputManager Events
    //:::::::::::::::::::::::::::::://

    private void InputManager_OnLook(InputManager.ActionPhase phase, InputManager.InputDevice inputDevice, Vector2 value) {
        _lookDelta = value;
    }

    private IEnumerator StartAimingCoroutine() {
        while (progress < transitionDuration) {
            // wait one frame
            yield return null;
            
            // increment progress
            progress += Time.deltaTime;

            // calculate t
            var t = progress / transitionDuration;
            
            // adjust camera settings
            _cinemachineThirdPersonFollow.CameraDistance = Mathf.Lerp(_cameraDistance, aimingCameraDistance, t);
            _cinemachineThirdPersonFollow.ShoulderOffset = Vector3.Lerp(_shoulderOffset, aimingShoulderOffset, t);
            _cinemachineThirdPersonFollow.CameraSide = Mathf.Lerp(0.5f, 1f, t);
        }

        // reset progress
        progress = transitionDuration;
    }

    private IEnumerator StopAimingCoroutine() {
        // if we have already stopped aiming, we're done
        if (progress <= 0f) yield break;
        
        // pause before returning to default settings
        yield return new WaitForSeconds(pauseDuration);
        
        while (0f < progress) {
            // wait one frame
            yield return null;
            
            // increment progress
            progress -= Time.deltaTime;

            // calculate t
            var t = progress / transitionDuration;
            
            // adjust camera settings
            _cinemachineThirdPersonFollow.CameraDistance = Mathf.Lerp(_cameraDistance, aimingCameraDistance, t);
            _cinemachineThirdPersonFollow.ShoulderOffset = Vector3.Lerp(_shoulderOffset, aimingShoulderOffset, t);
            _cinemachineThirdPersonFollow.CameraSide = Mathf.Lerp(0.5f, 1f, t);
        }

        // reset progress
        progress = 0f;
    }
}
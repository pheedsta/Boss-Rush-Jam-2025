using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: CrosshairCanvas
//++++++++++++++++++++++++++++++//

public class CrosshairCanvas : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Static Fields
    //:::::::::::::::::::::::::::::://

    public static CrosshairCanvas Instance;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://

    private GameObject _crosshairsGameObject;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://
    
    private void Awake() {
        if (!Instance) {
            Instance = this;
            Configure();
        } else {
            Destroy(this);
        }
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        _crosshairsGameObject = transform.Find("Crosshairs").gameObject;
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_crosshairsGameObject, "'Crosshairs' game object is missing");
        //++++++++++++++++++++++++++++++//
    }
    
    //------------------------------//
    // Crosshairs
    //------------------------------//

    public void ShowCrosshairs() {
        _crosshairsGameObject.SetActive(true);
    }

    public void HideCrosshairs() {
        _crosshairsGameObject.SetActive(false);
    }
}

using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: Health
//++++++++++++++++++++++++++++++//

public class CharacterHealth : MonoBehaviour {
    
    //------------------------------//
    // Properties
    //------------------------------//

    public bool IsAlive => 0f < _exactHealth;
    public int CurrentHealth => Mathf.CeilToInt(_exactHealth);
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [SerializeField] private int startingHealth = 100;
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float _exactHealth;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnEnable() {
        _exactHealth = startingHealth;
    }
    
    //------------------------------//
    // Damage Methods
    //------------------------------//

    public void ApplyDamage(float damage) {
        _exactHealth -= Mathf.Abs(damage);
    }
}

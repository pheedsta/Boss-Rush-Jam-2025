using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: Health
//++++++++++++++++++++++++++++++//

public class Health : MonoBehaviour {
    
    //------------------------------//
    // Delegates
    //------------------------------//

    public delegate void HealthEvent(int health);

    //------------------------------//
    // Events
    //------------------------------//
    
    public event HealthEvent OnChange = delegate { };
    
    //------------------------------//
    // Properties
    //------------------------------//

    public bool IsAlive => 0f < _exactHealth;
    public float HealthPercentage => Mathf.Clamp(_exactHealth / startingHealth, 0f, 1f);
    
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
    // Health Methods
    //------------------------------//

    private void AdjustHealth(float value) {
        // if value is zero; we're done
        if (Mathf.Approximately(value, 0f)) return;   
        
        // update health (with absolute value)
        _exactHealth += value;
        
        // clamp health to min / max
        _exactHealth = Mathf.Clamp(_exactHealth, 0f, startingHealth);
        
        // call OnChange event
        OnChange.Invoke(Mathf.CeilToInt(_exactHealth));
    }

    public void ApplyDamage(float damage) {
        AdjustHealth(-Mathf.Abs(damage));
    }

    public void ApplyHeal(float heal) {
        AdjustHealth(Mathf.Abs(heal));
    }
}

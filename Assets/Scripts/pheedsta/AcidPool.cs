using System.Collections;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: AcidPool
//++++++++++++++++++++++++++++++//

public class AcidPool : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Damage")]
    [Tooltip("Duration between damage effects")]
    [SerializeField] private float damageInterval;
    [Tooltip("Damage per tick")]
    [SerializeField] private float damagePerInterval;
    
    //:::::::::::::::::::::::::::::://
    // Unity Trigger Callbacks
    //:::::::::::::::::::::::::::::://
    
    private void OnTriggerEnter(Collider other) {
        // if this is the player entering, start damage coroutine
        var components = ComponentRegistry.ColliderComponents<Player>(other);
        if (0 < components.Length) StartCoroutine(DamageOverTimeCoroutine(components[0].CharacterHealth));
    }

    private void OnTriggerExit(Collider other) {
        // if this is the player exiting; stop coroutine
        var components = ComponentRegistry.ColliderComponents<Player>(other);
        if (0 < components.Length) StopAllCoroutines();
    }
    
    //:::::::::::::::::::::::::::::://
    // Coroutines
    //:::::::::::::::::::::::::::::://

    private IEnumerator DamageOverTimeCoroutine(CharacterHealth characterHealth) {
        // if a CharacterHealth wasn't passed, we're done
        if (!characterHealth) yield break;
        
        // initialise field
        var waitForSeconds = new WaitForSeconds(damageInterval);

        while (characterHealth.IsAlive) {
            // while the player is still alive; apply damage
            characterHealth.ApplyDamage(damagePerInterval);
            
            // wait for next interval
            yield return waitForSeconds;
        }
    }
}

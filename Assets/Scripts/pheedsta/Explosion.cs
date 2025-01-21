using DG.Tweening;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: Explosion
//++++++++++++++++++++++++++++++//

public class Explosion : MonoBehaviour, IReusable {
    
    //------------------------------//
    // IReusable Properties
    //------------------------------//

    public string Identifier => identifier;
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("IReusable")]
    [SerializeField] private string identifier;
    
    [Header("Explosion")]
    [SerializeField] private float maximumScale = 5f;
    [SerializeField] private float scaleUpDuration = 0.2f;
    [SerializeField] private float scaleDownDuration = 0.1f;

    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [SerializeField] private bool damagePlayer = true;
    
    [Header("Sound")]
    [SerializeField] private AK.Wwise.Event burnSound;
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly Collider[] _colliders = new Collider[30]; // allow explosion to hit 30 objects
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnEnable() {
        // return scale back to default
        transform.localScale = Vector3.one;
        
        // play burn sound
        burnSound.Post(gameObject);
        
        // scale up
        transform.DOScale(new Vector3(maximumScale, maximumScale, maximumScale), scaleUpDuration)
            .SetEase(Ease.OutCirc)
            .onComplete = OnScaleUpComplete;
    }
    
    //:::::::::::::::::::::::::::::://
    // Damage
    //:::::::::::::::::::::::::::::://

    private void ApplyDamage() {
        // if nothing is hit, we're done
        if (0 == Physics.OverlapSphereNonAlloc(transform.position, maximumScale / 2f, _colliders)) return;

        foreach (var hitCollider in _colliders) {
            if (damagePlayer) {
                // if this explosion is damaging the player, attempt to get the player from the collider
                var player = ComponentRegistry.ColliderComponent<Player>(hitCollider);

                // if a player was not returned, move on
                if (!player) continue;

                // a player was returned, apply damage and quit (we only want to apply damage once per explosion)
                player.Health.ApplyDamage(damage);
                break;
            }

            // TODO: we also need to damage the boss here as well!!!!!
            var skellyworm = ComponentRegistry.ColliderComponent<Skellyworm>(hitCollider);
            if (skellyworm) skellyworm.Health.ApplyDamage(damage);
        }
    }
    
    //:::::::::::::::::::::::::::::://
    // DOTween Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnScaleUpComplete() {
        // apply damage to player or enemies
        ApplyDamage();
        
        // scale down
        transform.DOScale(Vector3.zero, scaleDownDuration)
            .SetEase(Ease.OutCirc)
            .onComplete = OnScaleDownComplete;
    }

    private void OnScaleDownComplete() {
        // stop playing the burn sound
        burnSound.Stop(gameObject);
        
        // return to reusable pool
        ReusablePool.ReturnReusable(this);
    }
}

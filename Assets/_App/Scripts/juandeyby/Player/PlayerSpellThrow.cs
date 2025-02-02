using System;
using _App.Scripts.juandeyby;
using UnityEngine;

public class PlayerSpellThrow : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform origin;

    public void ThrowSpell()
    {
        var projectile = ServiceLocator.Get<FireProjectileManager>().GetProjectile();
        projectile.transform.position = origin.position;
        projectile.transform.rotation = origin.rotation;
        
        var force = transform.forward * 10f;
        projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        
        ServiceLocator.Get<MusicManager>().PlaySpellFire();
    }
}

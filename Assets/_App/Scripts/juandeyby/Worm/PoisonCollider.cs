using System;
using _App.Scripts.juandeyby;
using UnityEngine;

public class PoisonCollider : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ServiceLocator.Get<MusicManager>().PlayAcidHit();
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            var playerLocomotion = other.GetComponent<PlayerLocomotion>();
            playerLocomotion.Poison();
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

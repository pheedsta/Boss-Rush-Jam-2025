using System;
using _App.Scripts.juandeyby;
using UnityEngine;

public class PoisonCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ServiceLocator.Get<MusicManager>().PlayAcidHit();
            other.GetComponent<PlayerHealth>().TakeDamage(1);
            var playerLocomotion = other.GetComponent<PlayerLocomotion>();
            playerLocomotion.Poison();
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

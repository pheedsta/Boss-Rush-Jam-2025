using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BoosWeapon : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerHealth>().TakeDamage(10);
            }
        }
    }
}

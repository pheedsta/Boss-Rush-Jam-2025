using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class PlayerSword : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.MeleeDamage();
            }
        }
    }
}

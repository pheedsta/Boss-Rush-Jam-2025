using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class PlayerCollection : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerSpell playerSpell;
        
        public void Collect(Collectable collectable)
        {
            collectable.collectSound.Post(gameObject);
            if (collectable is CollectableHeart)
            {
                playerHealth.Heal();
            }
            else if (collectable is CollectableShard)
            {
                playerSpell.Charge();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace _App.Scripts.juandeyby.UI
{
    public class UIBossHealth : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        
        /// <summary>
        /// Set the health bar fill amount
        /// </summary>
        /// <param name="health"> The health value between 0 and 1 </param>
        public void SetHealth(float health)
        {
            healthBar.fillAmount = health;
        }
    }
}

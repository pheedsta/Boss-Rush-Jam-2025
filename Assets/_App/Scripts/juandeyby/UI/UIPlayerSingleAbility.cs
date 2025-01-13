using UnityEngine;
using UnityEngine.UI;

namespace _App.Scripts.juandeyby.UI
{
    public class UIPlayerSingleAbility : MonoBehaviour
    {
        [SerializeField] private Image abilityIcon;
        [SerializeField] private Image cooldownOverlay;
        
        /// <summary>
        /// Set the ability icon
        /// </summary>
        /// <param name="icon"> The sprite to set as the icon </param>
        public void SetAbilityIcon(Sprite icon)
        {
            abilityIcon.sprite = icon;
        }
        
        /// <summary>
        /// Set the cooldown overlay fill amount
        /// </summary>
        /// <param name="cooldown"> The cooldown value between 0 and 1 </param>
        public void SetCooldownOverlay(float cooldown)
        {
            cooldownOverlay.fillAmount = cooldown;
        }
    }
}

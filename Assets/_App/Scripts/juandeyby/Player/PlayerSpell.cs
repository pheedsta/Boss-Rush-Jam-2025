using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerSpell : MonoBehaviour
    {
        [SerializeField] private int maxCharges = 5;
        [SerializeField] private int maxEnergyCost = 100;
        [SerializeField] private int currentEnergy;
        [SerializeField] private int currentCharges;

        private void Start()
        {
            UpdateUI();
        }

        public void Charge()
        {
            currentEnergy += maxEnergyCost / 1;
            if (currentEnergy >= maxEnergyCost) {
                currentCharges++;
                currentEnergy = 0;
                if (currentCharges > maxCharges) {
                    currentCharges = maxCharges;
                }
            }
            UpdateUI();
        }
        
        public bool CanCastSpell()
        {
            if (currentCharges > 0)
            {
                currentCharges--;
                currentEnergy = 0;
                UpdateUI();
                return true;
            }
            return false;
        }
        
        public void LoseChange()
        {
            currentEnergy = 0;
            currentCharges = 0;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var uiValue = currentCharges / (float) maxCharges;
            UIServiceLocator.Get<UIManager>().HubPanel.PlayerSingleAbility.SetCooldownOverlay(uiValue);
        }

        public void Cast()
        {
            if (currentCharges > 0)
            {
                currentCharges--;
                // Cast spell
                Debug.Log("Spell casted!");
                // Update UI
                UIServiceLocator.Get<UIManager>().HubPanel.PlayerSingleAbility.SetCooldownOverlay(0f);
            }
        }
    }
}

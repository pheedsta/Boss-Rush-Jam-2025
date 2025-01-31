using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerSpell : MonoBehaviour
    {
        [SerializeField] private int maxCharges = 1;
        [SerializeField] private int maxEnergyCost = 100;
        [SerializeField] private int currentEnergy;
        [SerializeField] private int currentCharges;

        private void Start()
        {
            currentEnergy = 0;
            currentCharges = 0;
            UpdateUI();
        }

        public void Charge()
        {
            if (currentEnergy < maxEnergyCost)
            {
                currentEnergy += Mathf.CeilToInt(100 / 6f);
                if (currentEnergy > maxEnergyCost)
                {
                    currentEnergy = maxEnergyCost;
                }
                UpdateUI();
            }
            else
            {
                if (currentCharges < maxCharges)
                {
                    currentCharges++;
                }
            }
        }
        
        public void SpendChange()
        {
            currentEnergy = 0;
            currentCharges = 0;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var uiValue = currentEnergy / (float) maxEnergyCost;
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

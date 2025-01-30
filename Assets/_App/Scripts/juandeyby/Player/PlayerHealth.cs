using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int health;
        public int Health => health;

        private void Start()
        {
            health = maxHealth;
            UpdateUI();
        }

        public void Heal()
        {
            if (health < maxHealth)
            {
                health += 25;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            var uiValue = health / (float) maxHealth;
            UIServiceLocator.Get<UIManager>().HubPanel.PlayerHealth.SetHealth(uiValue);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                // Die();
            }
            
            var uiValue = health / (float) maxHealth;
            UIServiceLocator.Get<UIManager>().HubPanel.PlayerHealth.SetHealth(uiValue);
        }
    }
}

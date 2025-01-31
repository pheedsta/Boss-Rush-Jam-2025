using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private Transform spawnPointPhase1;
        [SerializeField] private Transform spawnPointPhase2;
        [SerializeField] private Transform spawnPointPhase3;
        
        [SerializeField] private PlayerSpell playerSpell;
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
        
        public void MaxHeal()
        {
            health = maxHealth;
            UpdateUI();
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
        
        
        private void Update()
        {
            if (transform.position.y < -10)
            {
                Spawn();
                MaxHeal();
                playerSpell.SpendChange();
            }
        }

        private void Spawn()
        {
            var phase = ServiceLocator.Get<GameManager>().GetGamePhase();
            switch (phase)
            {
                case GamePhase.Phase1:
                    transform.position = spawnPointPhase1.position;
                    transform.rotation = spawnPointPhase1.rotation;
                    break;
                case GamePhase.Phase2:
                    transform.position = spawnPointPhase2.position;
                    transform.rotation = spawnPointPhase2.rotation;
                    break;
                case GamePhase.Phase3:
                    transform.position = spawnPointPhase3.position;
                    transform.rotation = spawnPointPhase3.rotation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth = 100;
        private int _currentHealth;
        
        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        private void Start()
        {
            UIServiceLocator.Get<UIManager>().HubPanel.BossHealth.SetHealth(_currentHealth / (float) maxHealth);
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                // Die();
                UIServiceLocator.Get<UIManager>().ShowEndingPanel();
            }
            UIServiceLocator.Get<UIManager>().HubPanel.BossHealth.SetHealth(_currentHealth / (float) maxHealth);
        }
        
        public void MeleeDamage()
        {
            TakeDamage(4);
        }

        public void RangedDamage()
        {
            throw new System.NotImplementedException();
        }
    }
}

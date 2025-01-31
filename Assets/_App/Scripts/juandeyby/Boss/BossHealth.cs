using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private Boss boss;
        [SerializeField] private int maxHealth = 100;
        private int _currentHealth;
        private GameManager _gameManager;
        
        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        private void Start()
        {
            UIServiceLocator.Get<UIManager>().HubPanel.BossHealth.SetHealth(_currentHealth / (float) maxHealth);
            _gameManager = ServiceLocator.Get<GameManager>();
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                boss.SetState(new BossDeathState());
                UIServiceLocator.Get<UIManager>().ShowEndingPanel();
            }
            CheckPhase();
            UIServiceLocator.Get<UIManager>().HubPanel.BossHealth.SetHealth(_currentHealth / (float) maxHealth);
        }

        /// <summary>
        /// Check if the boss should change phase
        /// </summary>
        private void CheckPhase()
        {
            var phase = _gameManager.GetGamePhase();
            if (phase == GamePhase.Phase1)
            {
                if (_currentHealth <= 70)
                {
                    _gameManager.SetGamePhase(GamePhase.Phase2);
                }
            }
            else if (phase == GamePhase.Phase2)
            {
                if (_currentHealth <= 40)
                {
                    _gameManager.SetGamePhase(GamePhase.Phase3);
                }
            }
            else if (phase == GamePhase.Phase3)
            {
                // Do something
            }
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

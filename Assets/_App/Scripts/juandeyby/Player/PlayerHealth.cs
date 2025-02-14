using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator playerAnimator;
        
        [SerializeField] private Transform spawnPointPhase1;
        [SerializeField] private Transform spawnPointPhase2;
        [SerializeField] private Transform spawnPointPhase3;
        
        [SerializeField] private PlayerSpell playerSpell;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int health;
        
        [SerializeField] private List<Material> materials;
        
        public int Health => health;
        private bool _isDead;
        public bool IsDead => _isDead;
        private Coroutine _respawnCoroutine;
        
        private Coroutine _damageEffectCoroutine;
        
        private void Start()
        {
            foreach (var material in materials)
            {
                material.color = Color.white;
            }
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
            if (health <= 0 && _isDead == false)
            {
                health = 0;
                _isDead = true;
                playerAnimator.PlayTargetAnimation("Die", false);
                _respawnCoroutine = StartCoroutine(Respawn());
            }
            DamageEffect();
            ServiceLocator.Get<MusicManager>().PlayHurt();
            UpdateUI();
        }
        
        private void DamageEffect()
        {
            if (_damageEffectCoroutine != null)
            {
                StopCoroutine(_damageEffectCoroutine);
            }
            _damageEffectCoroutine = StartCoroutine(DamageEffectCoroutine());
        }

        private IEnumerator DamageEffectCoroutine()
        {
            foreach (var material in materials)
            {
                material.color = Color.red;
            }
            yield return new WaitForSeconds(0.1f);
            foreach (var material in materials)
            {
                material.color = Color.white;
            }
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(3);
            _isDead = false;
            Spawn();
            MaxHeal();
            playerSpell.LoseChange();
            playerAnimator.PlayTargetAnimation("Jump", false);
        }
        
        private void Update()
        {
            if (transform.position.y < -10)
            {
                Spawn();
                MaxHeal();
                playerSpell.LoseChange();
            }
        }

        private void Spawn()
        {
            var phase = ServiceLocator.Get<GameManager>().GetGamePhase();
            switch (phase)
            {
                case GamePhase.Phase1:
                    Debug.Log("Phase 1 spawn");
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

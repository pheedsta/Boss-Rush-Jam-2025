using System;
using System.Collections;
using System.Collections.Generic;
using _App.Scripts.juandeyby.UI;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class WormHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private List<Renderer> renderers;
        private Coroutine _damageCoroutine;
        
        [SerializeField] private int maxHealth = 3;
        private int _health;
        private UIHealthBar _uiHealthBar;

        private void OnEnable()
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
            }
            _health = maxHealth;
            _uiHealthBar = UIServiceLocator.Get<UIHealthCanvas>().GetHealthBar();
            _uiHealthBar.SetUnit(transform, Vector3.up * 0.5f);
            _uiHealthBar.SetHealth(_health / (float) maxHealth);
        }

        private void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                var worm = GetComponent<Worm>();
                ServiceLocator.Get<WormManager>().ReturnWorm(worm);
            }
            else
            {
                _uiHealthBar.SetHealth(_health / (float) maxHealth);
            }
        }

        public void MeleeDamage()
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
            }
            _damageCoroutine = StartCoroutine(DamageCoroutine());
            TakeDamage(1);
            
            // Play sound effect
            ServiceLocator.Get<MusicManager>().PlaySwordHit();
        }

        public void RangedDamage()
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
            }
            _damageCoroutine = StartCoroutine(DamageCoroutine());
            TakeDamage(3);
            
            // Play sound effect
            ServiceLocator.Get<MusicManager>().PlaySpellHit();
        }

        private IEnumerator DamageCoroutine()
        {
            var oldBlock = new MaterialPropertyBlock();
            var newBlock = new MaterialPropertyBlock();
            foreach (var renderer in renderers)
            {
                renderer.GetPropertyBlock(oldBlock);
                newBlock.SetColor("_BaseColor", Color.red);
                renderer.SetPropertyBlock(newBlock);
            }
            yield return new WaitForSeconds(0.1f);
            foreach (var renderer in renderers)
            {
                renderer.SetPropertyBlock(oldBlock);
            }
        }

        private void OnDisable()
        {
            UIServiceLocator.Get<UIHealthCanvas>().ReturnHealthBar(_uiHealthBar);
        }
    }
}

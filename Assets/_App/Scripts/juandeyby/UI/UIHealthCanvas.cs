using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby.UI
{
    [DefaultExecutionOrder(-100)]
    public class UIHealthCanvas : MonoBehaviour
    {
        [SerializeField] private UIHealthBar uiHealthBarPrefab;
        private readonly Queue<UIHealthBar> _healthBars = new Queue<UIHealthBar>();
        private readonly int _maxHealthBars = 20;

        private void OnEnable()
        {
            UIServiceLocator.Register<UIHealthCanvas>(this);
        }
        
        private void OnDisable()
        {
            UIServiceLocator.Unregister<UIHealthCanvas>();
        }

        private void Awake()
        {
            for (int i = 0; i < _maxHealthBars; i++)
            {
                var healthBar = Instantiate(uiHealthBarPrefab, transform);
                healthBar.gameObject.SetActive(false);
                _healthBars.Enqueue(healthBar);
            }
        }
        
        public UIHealthBar GetHealthBar()
        {
            if (_healthBars.Count == 0)
            {
                var healthBar = Instantiate(uiHealthBarPrefab, transform);
                _healthBars.Enqueue(healthBar);
            }

            var healthBarToReturn = _healthBars.Dequeue();
            healthBarToReturn.gameObject.SetActive(true);
            return healthBarToReturn;
        }
        
        public void ReturnHealthBar(UIHealthBar healthBar)
        {
            healthBar.gameObject.SetActive(false);
            _healthBars.Enqueue(healthBar);
        }
    }
}

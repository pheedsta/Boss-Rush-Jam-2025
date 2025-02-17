using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby.UI
{
    [DefaultExecutionOrder(-100)]
    public class UIHealthManager : MonoBehaviour
    {
        [SerializeField] private UIHealthBar uiHealthBarPrefab;
        private readonly Queue<UIHealthBar> _healthBars = new Queue<UIHealthBar>();
        private readonly int _maxHealthBars = 8;

        private void OnEnable()
        {
            UIServiceLocator.Register<UIHealthManager>(this);
        }
        
        private void OnDisable()
        {
            UIServiceLocator.Unregister<UIHealthManager>();
        }

        private void Awake()
        {
            for (int i = 0; i < _maxHealthBars; i++)
            {
                var healthBar = Instantiate(uiHealthBarPrefab, transform);
                healthBar.SetVisible(false);
                _healthBars.Enqueue(healthBar);
            }
        }
        
        public UIHealthBar GetHealthBar()
        {
            if (_healthBars.Count == 0)
            {
                var healthBar = Instantiate(uiHealthBarPrefab, transform);
                healthBar.SetVisible(false);
                _healthBars.Enqueue(healthBar);
            }

            var healthBarToReturn = _healthBars.Dequeue();
            healthBarToReturn.SetVisible(true);
            return healthBarToReturn;
        }
        
        public void ReturnHealthBar(UIHealthBar healthBar)
        {
            healthBar.SetVisible(false);
            _healthBars.Enqueue(healthBar);
        }
    }
}

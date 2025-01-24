using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class GameManager : MonoBehaviour
    {
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<GameManager>();
        }

        private void Awake()
        {
            Pause();
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }
        
        public void Resume()
        {
            Time.timeScale = 1;
        }
    }
}

using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _App.Scripts.juandeyby.UI
{
    public class UIHub : MonoBehaviour, IUIPanel
    {
        [SerializeField] private LeanWindow leanWindow;
        [SerializeField] private UIPlayerHealth playerHealth;
        public UIPlayerHealth PlayerHealth => playerHealth;
        [SerializeField] private UIPlayerSingleAbility playerSingleAbility;
        [SerializeField] private UIBossHealth bossHealth;
        public UIBossHealth BossHealth => bossHealth;
        public UIPlayerSingleAbility PlayerSingleAbility => playerSingleAbility;
        public void Show()
        {
            leanWindow.TurnOn();
            
            // Play Music
            ServiceLocator.Get<MusicManager>().Play();
        }

        public void Hide()
        {
            leanWindow.TurnOff();
        }

        private void Update()
        {
            if (leanWindow.On && Keyboard.current.pKey.wasPressedThisFrame)
            {
                ServiceLocator.Get<GameManager>().Pause();
                UIServiceLocator.Get<UIManager>().ShowPausePanel();
            }
        }
    }
}

using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _App.Scripts.juandeyby.UI
{
    public class UIStart : MonoBehaviour, IUIPanel
    {
        [SerializeField] private LeanButton startButton;
        [SerializeField] private LeanButton quitButton;
        [SerializeField] private LeanWindow leanWindow;

        private void Awake()
        {
            startButton.OnClick.AddListener(StartGame);
            quitButton.OnClick.AddListener(QuitGame);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            ServiceLocator.Get<GameManager>().Resume();
            UIServiceLocator.Get<UIManager>().ShowHubPanel();
        }

        public void Show()
        {
            leanWindow.TurnOn();
        }

        public void Hide()
        {
            if (leanWindow.On)
            {
                ServiceLocator.Get<MusicManager>().PlayBossMusic();
                ServiceLocator.Get<MusicManager>().OnPhase1();
                
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            leanWindow.TurnOff();
        }
    }
}

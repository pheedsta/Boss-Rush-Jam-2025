using Lean.Gui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _App.Scripts.juandeyby.UI
{
    public class UIPause : MonoBehaviour, IUIPanel
    {
        [SerializeField] private LeanWindow leanWindow;
    
        public void Show()
        {   
            leanWindow.TurnOn();
            
            // Play Music
            ServiceLocator.Get<MusicManager>().Pause();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Hide()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            leanWindow.TurnOff();
        }

        private void Update()
        {
            if (leanWindow.On && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ServiceLocator.Get<GameManager>().Resume();
                UIServiceLocator.Get<UIManager>().ShowHubPanel();
            }
        }
    }
}

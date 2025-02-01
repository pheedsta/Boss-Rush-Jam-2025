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
        }

        public void Hide()
        {
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

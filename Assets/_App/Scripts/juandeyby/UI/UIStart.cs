using Lean.Gui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _App.Scripts.juandeyby.UI
{
    public class UIStart : MonoBehaviour, IUIPanel
    {
        [SerializeField] private LeanWindow leanWindow;
    
        public void Show()
        {
            leanWindow.TurnOn();
        }

        public void Hide()
        {
            leanWindow.TurnOff();
        }

        private void Update()
        {
            if (leanWindow.On && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                ServiceLocator.Get<GameManager>().Resume();
                UIServiceLocator.Get<UIManager>().ShowHubPanel();
            }
        }
    }
}

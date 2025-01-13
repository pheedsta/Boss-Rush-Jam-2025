using Lean.Gui;
using UnityEngine;

namespace _App.Scripts.juandeyby.UI
{
    public class UIHub : MonoBehaviour, IUIPanel
    {
        [SerializeField] private LeanWindow leanWindow;
        [SerializeField] private UIPlayerHealth playerHealth;
        public UIPlayerHealth PlayerHealth => playerHealth;
        [SerializeField] private UIPlayerSingleAbility playerSingleAbility;
        public UIPlayerSingleAbility PlayerSingleAbility => playerSingleAbility;
        public void Show()
        {
            leanWindow.TurnOn();
        }

        public void Hide()
        {
            leanWindow.TurnOff();
        }
    }
}

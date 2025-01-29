using _App.Scripts.juandeyby;
using _App.Scripts.juandeyby.UI;
using Lean.Gui;
using UnityEngine;

public class UIEnding : MonoBehaviour, IUIPanel
{
    [SerializeField] private LeanWindow leanWindow;
    
    public void Show()
    {
        leanWindow.TurnOn();
        ServiceLocator.Get<GameManager>().Pause();
    }
    
    public void Hide()
    {
        leanWindow.TurnOff();
    }
}
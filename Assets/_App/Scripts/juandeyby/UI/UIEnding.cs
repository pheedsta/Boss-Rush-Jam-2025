using _App.Scripts.juandeyby.UI;
using Lean.Gui;
using UnityEngine;

public class UIEnding : MonoBehaviour, IUIPanel
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
}
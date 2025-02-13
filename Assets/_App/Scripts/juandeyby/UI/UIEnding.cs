using System;
using System.Collections;
using _App.Scripts.juandeyby;
using _App.Scripts.juandeyby.UI;
using Lean.Gui;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEnding : MonoBehaviour, IUIPanel
{
    [SerializeField] private LeanWindow leanWindow;
    private Coroutine _animationCoroutine;
    
    public void Show()
    {
        _animationCoroutine = StartCoroutine(Animate());
    }
    
    public void Hide()
    {
        leanWindow.TurnOff();
    }
    
    private IEnumerator Animate()
    {
        yield return new WaitForSeconds(3f);
        leanWindow.TurnOn();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ServiceLocator.Get<GameManager>().Pause();
    }

    private void OnDisable()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
    }
}
using System;
using _App.Scripts.juandeyby;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void OnEnable()
    {
        UIServiceLocator.Register<UIManager>(this);
    }
    
    private void OnDisable()
    {
        UIServiceLocator.Unregister<UIManager>();
    }
}

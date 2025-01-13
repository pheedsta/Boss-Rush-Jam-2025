using System;
using _App.Scripts.juandeyby;
using _App.Scripts.juandeyby.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIStart startPanel;
    [SerializeField] private UIPause pausePanel;
    [SerializeField] private UIHub hubPanel;
    [SerializeField] private UIEnding endingPanel;
    
    private void OnEnable()
    {
        UIServiceLocator.Register<UIManager>(this);
    }
    
    private void OnDisable()
    {
        UIServiceLocator.Unregister<UIManager>();
    }
 
    public void ShowStartPanel()
    {
        startPanel.Show();
        pausePanel.Hide();
        hubPanel.Hide();
        endingPanel.Hide();
    }
    
    public void ShowPausePanel()
    {
        startPanel.Hide();
        pausePanel.Show();
        hubPanel.Hide();
        endingPanel.Hide();
    }
    
    public void ShowHubPanel()
    {
        startPanel.Hide();
        pausePanel.Hide();
        hubPanel.Show();
        endingPanel.Hide();
    }
    
    public void ShowEndingPanel()
    {
        startPanel.Hide();
        pausePanel.Hide();
        hubPanel.Hide();
        endingPanel.Show();
    }
}

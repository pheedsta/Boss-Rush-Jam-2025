using System;
using _App.Scripts.juandeyby;
using _App.Scripts.juandeyby.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIStart startPanel;
    public UIStart StartPanel => startPanel;
    [SerializeField] private UIPause pausePanel;
    public UIPause PausePanel => pausePanel;
    [SerializeField] private UIHub hubPanel;
    public UIHub HubPanel => hubPanel;
    [SerializeField] private UIEnding endingPanel;
    public UIEnding EndingPanel => endingPanel;
    
    private void OnEnable()
    {
        UIServiceLocator.Register<UIManager>(this);
    }
    
    private void OnDisable()
    {
        UIServiceLocator.Unregister<UIManager>();
    }
 
    /// <summary>
    /// Show the start panel
    /// </summary>
    public void ShowStartPanel()
    {
        startPanel.Show();
        pausePanel.Hide();
        hubPanel.Hide();
        endingPanel.Hide();
    }
    
    /// <summary>
    /// Show the pause panel
    /// </summary>
    public void ShowPausePanel()
    {
        startPanel.Hide();
        pausePanel.Show();
        hubPanel.Hide();
        endingPanel.Hide();
    }
    
    /// <summary>
    /// Show the hub panel
    /// </summary>
    public void ShowHubPanel()
    {
        startPanel.Hide();
        pausePanel.Hide();
        hubPanel.Show();
        endingPanel.Hide();
    }
    
    /// <summary>
    /// Show the ending panel
    /// </summary>
    public void ShowEndingPanel()
    {
        startPanel.Hide();
        pausePanel.Hide();
        hubPanel.Hide();
        endingPanel.Show();
    }
}

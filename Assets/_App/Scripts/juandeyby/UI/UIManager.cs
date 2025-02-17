using System;
using _App.Scripts.juandeyby;
using _App.Scripts.juandeyby.UI;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
    [SerializeField] private UIPreStart preStartPanel;
    public UIPreStart PreStartPanel => preStartPanel;
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
        preStartPanel.Hide();
        startPanel.Show();
        // pausePanel.Hide();
        // hubPanel.Hide();
        // endingPanel.Hide();
    }

    /// <summary>
    /// Show the pause panel
    /// </summary>
    public void ShowPausePanel()
    {
        // preStartPanel.Hide();
        // startPanel.Hide();
        pausePanel.Show();
        hubPanel.Hide();
        // endingPanel.Hide();
    }

    /// <summary>
    /// Show the hub panel
    /// </summary>
    public void ShowHubPanel()
    {
        // preStartPanel.Hide();
        startPanel.Hide();
        pausePanel.Hide();
        hubPanel.Show();
        // endingPanel.Hide();
    }

    /// <summary>
    /// Show the ending panel
    /// </summary>
    public void ShowEndingPanel()
    {
        // preStartPanel.Hide();
        // startPanel.Hide();
        // pausePanel.Hide();
        hubPanel.Hide();
        endingPanel.Show();
    }
}
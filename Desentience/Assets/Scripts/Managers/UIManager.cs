using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=hjnJnz77OVU
[RequireComponent(typeof(DDOL))]
public class UIManager : MonoBehaviour
{

    private static UIManager instance;
    //Public getter for instance
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Pause_UIPanel pauseMenu;
    public Main_UIPanel mainMenu;
    public GameOver_UIPanel gameOverMenu;
    public LevelComplete_UIPanel levelCompleteMenu;

    [SerializeField]
    private Base_UIPanel _currentPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TriggerOpenPanel(mainMenu);
    }

    private void Update()
    {
        if (_currentPanel) {
            _currentPanel.UpdateBehavior();
        };
    }

    public void TriggerPanelTransition(Base_UIPanel panel)
    {
        if (panel == null)
        {
            TriggerClosePanel(_currentPanel);
            _currentPanel = null;
        } else
        {
            TriggerOpenPanel(panel);
        }
    }

    void TriggerOpenPanel(Base_UIPanel panel)
    {
        if (_currentPanel != null) TriggerClosePanel(_currentPanel);
        _currentPanel = panel;
        _currentPanel.OpenBehavior();
    }

    void TriggerClosePanel(Base_UIPanel panel)
    {
        panel.CloseBehavior();
    }

}
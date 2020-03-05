using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public Base_UIPanel firstPanel;
    public Base_UIPanel secondPanel;

    Base_UIPanel _currentPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        TriggerOpenPanel(firstPanel);
    }

    private void Update()
    {
        if (_currentPanel) _currentPanel.UpdateBehavior();
    }

    public void TriggerPanelTransition(Base_UIPanel panel)
    {
        TriggerOpenPanel(panel);
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

    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

}
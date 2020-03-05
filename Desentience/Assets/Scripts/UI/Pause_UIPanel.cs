using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class Pause_UIPanel : Base_UIPanel
{
    public Button resumeButton;
    public Button menuButton;
    public override void OpenBehavior()
    {
        base.OpenBehavior();
        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() => { ResumeButtonPressed(); });
    }

    private void Awake()
    {
        
    }

    void ResumeButtonPressed()
    {
        UIManager.Instance.TogglePause();
    }

    void MenuButtonPressed()
    {
        
    }
}
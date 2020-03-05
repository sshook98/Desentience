using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class Main_UIPanel : Base_UIPanel
{
    public Button startButton;
    public override void OpenBehavior()
    {
        base.OpenBehavior();
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => { StartButtonPressed(); });
    }

    void StartButtonPressed()
    {
        Base_UIPanel nextPanel = UIManager.instance.secondPanel;
        UIManager.Instance.TriggerPanelTransition(nextPanel);
    }
}
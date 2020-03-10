using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class Main_UIPanel : Base_UIPanel
{
    public Button startButton;
    public Button quitButton;
    public override void OpenBehavior()
    {
        base.OpenBehavior();
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => { StartButtonPressed(); });
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => { QuitButtonPressed(); });
    }

    void StartButtonPressed()
    {
        GameManager.Instance.StartGame();
    }

    void QuitButtonPressed()
    {
        GameManager.Instance.QuitGame();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class LevelComplete_UIPanel : Base_UIPanel
{
    public Button nextLevelButton;
    public Button mainMenuButton;
    public Button quitButton;

    public override void OpenBehavior()
    {
        base.OpenBehavior();
        nextLevelButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.AddListener(() => { NextLevelButtonPressed(); });
        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() => { MainMenuButtonPressed(); });
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => { QuitButtonPressed(); });
    }

    void NextLevelButtonPressed()
    {
        GameManager.Instance.StartGame();
    }

    void MainMenuButtonPressed()
    {
        GameManager.Instance.ReturnToMainMenu();
    }

    void QuitButtonPressed()
    {
        GameManager.Instance.QuitGame();
    }
}

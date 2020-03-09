using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class GameOver_UIPanel : Base_UIPanel
{
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;

    public override void OpenBehavior()
    {
        base.OpenBehavior();
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => { RestartButtonPressed(); });
        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() => { MainMenuButtonPressed(); });
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => { QuitButtonPressed(); });
    }

    void RestartButtonPressed()
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

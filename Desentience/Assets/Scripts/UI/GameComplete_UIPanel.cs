using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameComplete_UIPanel : Base_UIPanel
{
    public Button mainMenuButton;
    public Button quitButton;

    public override void OpenBehavior()
    {
        base.OpenBehavior();
        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() => { MainMenuButtonPressed(); });
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => { QuitButtonPressed(); });
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

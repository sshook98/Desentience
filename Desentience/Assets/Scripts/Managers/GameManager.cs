using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DDOL))]
public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    //Neat lil C# getter for the GameManager instance
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    //References we want stored in the GameManager
    //GameObjects assign themselves during Awake(), since the GameManager is pre-loaded and thus already awake
    //Game flow is:
    //  TitleScreen is loaded, GameManager and UIManager Awake()
    //  Player presses "Wake Up"
    //    -Can also make this automatic when using the editor, for ease of testing
    //  Scene specified by 'firstScene' string is loaded
    //  All monobehaviors Awake()
    //    -This is where they assign themselves to the manager
    //  Monobehaviors call Start() when first used. It is safe to use the GameManager at this point, as everything has been assigned (barring bugs)
    public GameObject player;

    //TODO
    //Add a public enum corresponding to each testing scene and the actual FirstLevel scene
    //This would let us change which scene will load from a drop-down in the editor

    // public Inventory inventory;

    // Use unity inspector to change this array of levels
    [SerializeField]
    private string[] levelNames;
    private string titleScreen = "TitleScreen";

    [SerializeField]
    private bool isPaused = false;
    public bool IsPaused()
    {
        return isPaused;
    }

    public bool keyCardCollected = false;
    public GameObject keyCard;
    public ElevatorController elevator;    

    //Awake is always called before any Start functions
    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape") && SceneManager.GetActiveScene().name != "TitleScreen")
        {
            TogglePause();
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            UIManager.Instance.TriggerPanelTransition(null);
            Time.timeScale = 1.0f;
            isPaused = false;
        } else
        {
            Time.timeScale = 0.0f;
            UIManager.Instance.TriggerPanelTransition(UIManager.Instance.pauseMenu);
            isPaused = true;
        }
    }

    private int currentLevel;
    public void StartGame()
    {
        LoadScene(levelNames[0]);
        currentLevel = 0;
        UIManager.Instance.TriggerPanelTransition(null);
    }

    public void QuitGame()
    {
        Debug.Log("Quittin' time");
        Application.Quit();
    }

    public void PlayerDeath()
    {
        Debug.Log("Player Died");
        UIManager.Instance.TriggerPanelTransition(UIManager.Instance.gameOverMenu);
    }

    public void ReturnToMainMenu()
    {
        LoadScene(titleScreen);
        Time.timeScale = 1.0f;
        isPaused = false;
        UIManager.Instance.TriggerPanelTransition(UIManager.Instance.mainMenu);
    }

    public void CollectKeyCard() 
    {
        keyCardCollected = true;
        elevator.activateElevator();
        keyCard.SetActive(false);
    }

    public bool IsElevatorAvailable()
    {
        if (elevator != null)
        {
            return elevator.isElevatorActivated();
        }

        return false;
    }

    public void HandleLevelComplete()
    {
        // show win screen here or load a different level
        UIManager.Instance.TriggerPanelTransition(UIManager.Instance.levelCompleteMenu);
    }

    public void NextLevel()
    {
        if (currentLevel + 1 >= levelNames.Length)
        {
            ShowVictoryScreen();
        }
        else
        {
            currentLevel += 1;
            LoadScene(levelNames[currentLevel + 1]);
        }
    }

    public void ShowVictoryScreen()
    {

    }
}
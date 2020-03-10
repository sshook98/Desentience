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
    //  Scene specified by 'gameScene' string is loaded
    //  All monobehaviors Awake()
    //    -This is where they assign themselves to the manager
    //  Monobehaviors call Start() when first used. It is safe to use the GameManager at this point, as everything has been assigned (barring bugs)
    public GameObject player;

    //TODO
    //Add a public enum corresponding to each testing scene and the actual FirstLevel scene
    //This would let us change which scene will load from a drop-down in the editor

    //Right now, change gameScene to the name of the scene you want to load
    [SerializeField]
    private string gameScene = "ZachTestingScene";
    [SerializeField]
    private bool isPaused = false;

    public bool keyCardCollected = false;
    public ElevatorController elevator;    

    //Awake is always called before any Start functions
    void Awake()
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

        //TODO
        //*
        //Move these to Awake() of their respective scripts
        /**
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Could not find player object in scene, should be tagged as Player");
            }
        }

        if (elevator == null)
        {
            GameObject go = GameObject.FindWithTag("Elevator");

            if (go != null && go.GetComponent<ElevatorController>() != null)
            {
                elevator = go.GetComponent<ElevatorController>();
            } else
            {
                Debug.LogError("Could not find elevator object, should be tagged as Elevator");
            }
        }
        //Move these to Awake() of their respective scripts
        **/
        //*
        //TODO

    public bool IsPaused()
    {
        return isPaused;
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape") && SceneManager.GetActiveScene().name != "TitleScreen")
        {
            TogglePause();
        }
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

    public void StartGame()
    {
        LoadScene(gameScene);
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
        LoadScene("TitleScreen");
        Time.timeScale = 1.0f;
        isPaused = false;
        UIManager.Instance.TriggerPanelTransition(UIManager.Instance.mainMenu);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CollectKeyCard() 
    {
        keyCardCollected = true;
        elevator.activateElevator();
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

}
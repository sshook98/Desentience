using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DDOL))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //References we want stored in the GameManager
    public GameObject player;

    [SerializeField]
    private string gameScene = "ZachTestingScene";
    [SerializeField]
    private bool isPaused = false;

    private ElevatorController elevator;
    private KeyCardController keyCard;

    private bool keyCardCollected = false;

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
            if (go != null)
            {
                elevator = go.GetComponent<ElevatorController>();
            }

            if (elevator == null)
            {
                Debug.LogError("Could not find elevator object in scene, should be tagged as Elevator");
            }
        }

        if (keyCard == null)
        {
            GameObject go = GameObject.FindWithTag("KeyCard");
            if (go != null)
            {
                keyCard = go.GetComponent<KeyCardController>();
            }
        }

    }



    public bool IsPaused()
    {
        return isPaused;
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
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
        LoadScene("ZachTestingScene");
        UIManager.Instance.TriggerPanelTransition(null);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static GameManager Instance
    {
        get {
            return instance;
        }
    }

    public void CollectKeyCard()
    {
        keyCardCollected = true;

        if (elevator != null)
        {
            elevator.ActivateElevator();
        }
    }

    public bool IsElevatorAvailable()
    {
        if (elevator != null)
        {
            return elevator.IsElevatorAvailable();
        }

        return false;
    }

    public void HandleLevelComplete()
    {
        // show win screen here or load a different level

        Debug.Log("Level Complete!");
    }





}
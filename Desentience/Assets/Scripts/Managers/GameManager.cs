﻿using UnityEngine;
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

    public bool keyCardCollected = false;
    private ElevatorController elevator;    

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

    private void Start()
    {
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
        elevator.actiateElevator();
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
        Debug.Log("Level Complete");
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("Player died");
    }
}
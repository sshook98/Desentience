﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;        //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    //Component references we want stored in the GameManager
    public GameObject player = null;

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
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to each of the private variables
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("No GameObject found with Player tag");
        }

    }

    public static GameManager Instance
    {
        get {
            return instance;
        }
    }

}
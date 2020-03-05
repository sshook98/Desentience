using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    //Component references we want stored in the GameManager
    public GameObject player;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Get a component reference to each of the private variables
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("No GameObject found with Player tag");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            UIManager.Instance.TogglePause();
        }

    }


    public static GameManager Instance
    {
        get {
            return instance;
        }
    }

}
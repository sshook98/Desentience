using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void loadScene(string sceneName)
    {
        int sceneCount = SceneManager.sceneCount;
        bool foundSceneName = false;
        for (int i = 0; i < sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "SceneName")
            {
                foundSceneName = true;
                break;
            }
        }

        if (foundSceneName)
        {
            SceneManager.LoadScene(sceneName);
        } else
        {
            Debug.LogError("Incorrect scene name: " + sceneName);
        }
    }
}

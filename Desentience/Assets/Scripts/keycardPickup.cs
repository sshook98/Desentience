using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycardPickup : MonoBehaviour
{

    private void Awake()
    {
        GameManager.Instance.keyCard = gameObject;
    }
    void Update()
    {
        /**
        if (GameManager.Instance.keyCardCollected == true) {
            this.gameObject.SetActive(false);
        }
        **/
    }
}

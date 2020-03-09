using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycardPickup : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.keyCardCollected == true) {
            this.gameObject.SetActive(false);
        }
    }
}

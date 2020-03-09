using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycardPickup : MonoBehaviour
{
    public GameObject card;

    // Start is called before the first frame update
    void Start()
    {
        GameObject card = GameObject.FindGameObjectWithTag("KeyCard");
    }

    // Update is called once per frame
    void Update()
    {
        if (card == null) {
            Destroy(this.gameObject);
            print("heyy");
        }
    }
}

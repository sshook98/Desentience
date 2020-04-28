using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public int damage = 40;
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

}

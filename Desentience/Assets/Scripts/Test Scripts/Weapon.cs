using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IActionable
{
 
    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual bool Action()
    {
        return false;
    }
}

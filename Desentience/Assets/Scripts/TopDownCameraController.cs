using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    private void Awake()
    {
        
    }
    private void Start()
    {
        target = GameManager.Instance.player.transform;
        // Don't put anything after this, stops execution at this line for whatever reason
        offset = transform.position - target.position;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}

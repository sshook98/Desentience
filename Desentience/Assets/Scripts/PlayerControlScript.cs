using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class PlayerControlScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private CharacterInputController cinput;

    public float forwardMaxSpeed = 1f;
    public float turnMaxSpeed = 1f;

    public GameObject projectile;
    public float projectileSpeed = 20;
    public int projectileDelay = 50;
    public float destroyDelay = 3.0f;

    private int fireDelay;

    private Renderer[] rends;
    private Bounds allBounds;

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();

        if (cinput == null)
            Debug.Log("CharacterInputController could not be found");

    }


    // Use this for initialization
    void Start()
    {
        anim.applyRootMotion = false;
        fireDelay = 0;

        //get bounds of renderer and renderers of all children
        rends = GetComponentsInChildren<Renderer>();
        allBounds.center = this.transform.position;
        foreach (Renderer rend in rends)
        {
            allBounds.Encapsulate(rend.bounds);
        }

    }



    void Update()
    {

        float inputForward = 0f;
        float inputTurn = 0f;
        bool firing = false;

        if (cinput.enabled)
        {
            inputForward = cinput.Forward;
            inputTurn = cinput.Turn;
            firing = cinput.Action;
        }

        rbody.MovePosition(rbody.position + this.transform.forward * inputForward * Time.deltaTime * forwardMaxSpeed);

        rbody.MoveRotation(rbody.rotation * Quaternion.AngleAxis(inputTurn * Time.deltaTime * turnMaxSpeed, Vector3.up));

        if (firing && fireDelay <= 0)
        {
            Vector3 projectilePosition = transform.position + (transform.up * (allBounds.size.y / 2)) + (transform.forward * allBounds.size.z);
            GameObject instantiatedProjectile = Instantiate(projectile, projectilePosition, transform.rotation);
            
            instantiatedProjectile.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, projectileSpeed));
            instantiatedProjectile.GetComponent<BooletScript>().destroyDelay = destroyDelay;
            fireDelay = projectileDelay;

        } else if (fireDelay > 0)
        {
            fireDelay--;
        }


        anim.SetFloat("velx", inputTurn);
        anim.SetFloat("vely", inputForward);

    }

}

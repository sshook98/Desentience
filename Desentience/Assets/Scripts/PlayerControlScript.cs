using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class PlayerControlScript : MonoBehaviour
{
    public float forwardMaxSpeed = 1f;
    public float turnMaxSpeed = 1f;
    public GameObject projectile;
    public float projectileSpeed = 20;
    public int projectileDelay = 50;
    public float destroyDelay = 3.0f;
    public int fireRate;

    private Renderer[] rends;
    private Bounds allBounds;
    private Animator anim;
    private Rigidbody rb;
    private CharacterInputController cinput;

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.Log("Animator could not be found");
        }

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("Rigid body could not be found");
        }

        cinput = GetComponent<CharacterInputController>();

        if (cinput == null)
        {
            Debug.Log("CharacterInputController could not be found");
        }

        GameManager.Instance.player = gameObject;
    }


    void Start()
    {
        anim.applyRootMotion = false;

        ////get bounds of renderer and renderers of all children
        //rends = GetComponentsInChildren<Renderer>();
        //allBounds.center = this.transform.position;
        //foreach (Renderer rend in rends)
        //{
        //    allBounds.Encapsulate(rend.bounds);
        //}

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

        rb.MovePosition(rb.position + this.transform.forward * inputForward * Time.deltaTime * forwardMaxSpeed);

        rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(inputTurn * Time.deltaTime * turnMaxSpeed, Vector3.up));

        if (firing && fireRate <= 0)
        {
            Vector3 projectilePosition = transform.position + (transform.up * (allBounds.size.y / 2)) + (transform.forward * allBounds.size.z);
            GameObject instantiatedProjectile = Instantiate(projectile, projectilePosition, transform.rotation);
            
            instantiatedProjectile.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, projectileSpeed));
            instantiatedProjectile.GetComponent<BooletScript>().destroyDelay = destroyDelay;
            fireRate = projectileDelay;

        } else if (fireRate > 0)
        {
            fireRate--;
        }


        anim.SetFloat("velx", inputTurn);
        anim.SetFloat("vely", inputForward);

    }

}

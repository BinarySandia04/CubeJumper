﻿using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public GameObject PlayerModel;
    public Transform spawnpoint;
    private Rigidbody playerRigidbody;
    public GameObject decal;
    [Space]
    public Vector3 forwardSpeed, backwardSpeed, leftSpeed, rightSpeed;
    [Space]
    public float jumpSpeed = 10f;
    public float deacceleration = 0.8f;
    private float ySpeed = 0f;
    [HideInInspector]
    public Vector3 constantPlayerSpeed = new Vector3();
    public Vector3 beforeConstantSpeed = new Vector3();
    private bool jumping = false;
    private bool walled;
    private float distanciaAlSuelo;
    public float platforming = 10f;
    public float currentTimeInAir = -1f;

    public float airControl = 2f;

    Vector3 platformMomentum = Vector3.zero;

    private MovingPlatformController mpc = null;

    ColliderManager lCollider;
    ColliderManager fCollider;
    ColliderManager rCollider;
    ColliderManager bCollider;
    ColliderManager dCollider;

    private void Awake()
    {
        lCollider = transform.Find("Colliders").Find("Left").GetComponent<ColliderManager>();
        rCollider = transform.Find("Colliders").Find("Right").GetComponent<ColliderManager>();
        fCollider = transform.Find("Colliders").Find("Front").GetComponent<ColliderManager>();
        bCollider = transform.Find("Colliders").Find("Back").GetComponent<ColliderManager>();
        dCollider = transform.Find("Colliders").Find("Down").GetComponent<ColliderManager>();
    }

    bool notCollidingWalls()
    {
        if (lCollider.collided || rCollider.collided || fCollider.collided || bCollider.collided) return false;
        else return true;
    }

    private void checkInput()
    {
        if (Input.GetKey(KeyCode.W))
        { // - Z
            constantPlayerSpeed += forwardSpeed;  
        }
        if (Input.GetKey(KeyCode.A))
        { // + X
            constantPlayerSpeed += leftSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        { // + Z
            constantPlayerSpeed += backwardSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        { // - X
            constantPlayerSpeed += rightSpeed;
            
        }
        if (Input.GetKey(KeyCode.Space) && walled && notCollidingWalls())
        {
            jumping = true;
            GetComponent<Rigidbody>().AddForce(0, jumpSpeed, 0, ForceMode.VelocityChange);
            walled = false;
        } else
        {
            jumping = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawnpoint.position;
            constantPlayerSpeed = Vector3.zero;
            platformMomentum = Vector3.zero;
            playerRigidbody.velocity = Vector3.zero;
        }
    }

    void Start()
    {
        // Nada?
        currentTimeInAir = 0f;
        StartCoroutine(makeDecals());
        playerRigidbody = PlayerModel.GetComponent<Rigidbody>();
        distanciaAlSuelo = GetComponent<Collider>().bounds.extents.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Contains("Trigger"))
        {
            if(dCollider.collided) walled = true;

            if (collision.gameObject.name == "Platform")
            {
                mpc = collision.transform.parent.GetComponent<MovingPlatformController>();
            }
            else
            {
                platformMomentum = Vector3.zero;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.name.Contains("Trigger"))
        {
            if (dCollider.collided) walled = true;

            if (collision.gameObject.name == "Platform")
            {
                mpc = collision.transform.parent.GetComponent<MovingPlatformController>();
            }
            else
            {
                platformMomentum = Vector3.zero;
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.name.Contains("Trigger"))
        {
            walled = false;
            if (collision.gameObject.name == "Platform")
            {
                mpc = null;
            }
        }
        
    }

    // Rigidbody y esas cosas raras
    void FixedUpdate()
    {
        if (walled)
        {
            currentTimeInAir = 0;
        } else
        {
            if (currentTimeInAir < 10f)
            {
                currentTimeInAir += Time.deltaTime;
            }
        }
        

        checkInput();
        // Aplicamos la fuerza y desaceleramos
        if (walled)
        {
            constantPlayerSpeed = Vector3.Lerp(constantPlayerSpeed, Vector3.zero, deacceleration);
        } else
        {
            constantPlayerSpeed = Vector3.Lerp(Vector3.Lerp(constantPlayerSpeed, Vector3.zero, currentTimeInAir / 10), Vector3.zero, deacceleration);
        }
        
        constantPlayerSpeed.y = 0;
        
        transform.Translate(constantPlayerSpeed / 10);

        if (mpc != null)
        {
            if (!mpc.stopped) platformMomentum = (mpc.motion);
        }

        transform.position += (platformMomentum);
        // Mover el desto al otro
    }

    IEnumerator makeDecals()
    {
        while (true)
        {
            if (walled)
            {
                GameObject newObject = Instantiate(decal, transform);
                if (mpc != null) newObject.transform.SetParent(mpc.transform.Find("Platform"));
                else newObject.transform.SetParent(null);
                Destroy(newObject, 180);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}

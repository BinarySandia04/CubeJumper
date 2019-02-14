using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour {

    [Header("Key binds")]
    public KeyCode forward;
    public KeyCode back;
    public KeyCode left;
    public KeyCode right;
    [Space(20)]
    public float forwardSpeed;
    public float lateralSpeed;
    public Vector3 maxSpeed = new Vector3();
    [Space]
    public float deaccelerationFloat;
    Rigidbody rb;
    Vector3 currentForce = new Vector3();
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate ()
    {
        if (GetKey(forward) && currentForce.x > maxSpeed.x * -1)
        {
            currentForce.x -= forwardSpeed;
        }
        if (GetKey(back) && currentForce.x < maxSpeed.x)
        {
            currentForce.x += forwardSpeed;
        }
        if (GetKey(left) && currentForce.z > maxSpeed.z * -1)
        {
            currentForce.z -= lateralSpeed;
        }
        if (GetKey(right) && currentForce.z < maxSpeed.z)
        {
            currentForce.z += lateralSpeed;
        }
        Deaccelerate();
        rb.velocity = currentForce;
    }

    private void Deaccelerate()
    {
        Vector3 quiet = new Vector3(); // 0,0,0
        float finalX = Mathf.Lerp(currentForce.x, quiet.x, deaccelerationFloat);
        float finalZ = Mathf.Lerp(currentForce.z, quiet.z, deaccelerationFloat);
        currentForce.x = finalX;
        currentForce.z = finalZ;
    }

    private bool GetKey(KeyCode k)
    {
        if (Input.GetKey(k)) return true;
        else return false;
    }
}

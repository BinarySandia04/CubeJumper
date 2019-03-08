using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public GameObject PlayerModel;
    private Rigidbody playerRigidbody;
    [Space]
    public Vector3 forwardSpeed, backwardSpeed, leftSpeed, rightSpeed;
    [Space]
    public float jumpSpeed = 10f;
    public float deacceleration = 0.8f;
    private float ySpeed = 0f;
    [HideInInspector]
    public Vector3 constantPlayerSpeed = new Vector3();
    private bool jumping = false;
    private bool walled;

    private void checkInput()
    {
        if (Input.GetKey(KeyCode.W))
        { // - Z
            float distance = 1000f;
            RaycastHit hit;
            // Cast
            if (Physics.SphereCast(PlayerModel.transform.position, 1f, new Vector3(0, 0, -1), out hit))
            {
                distance = hit.distance;
            }
            if (distance > 0.3f)
            {
                constantPlayerSpeed += forwardSpeed;
            }
            
        }
        if (Input.GetKey(KeyCode.A))
        { // + X
            float distance = 1000f;
            RaycastHit hit;
            // Cast
            if (Physics.SphereCast(PlayerModel.transform.position, 1f, new Vector3(1, 0, 0), out hit))
            {
                distance = hit.distance;
            }
            if (distance > 0.2f)
            {
                constantPlayerSpeed += leftSpeed;
            }
        }
        if (Input.GetKey(KeyCode.S))
        { // + Z
            float distance = 1000f;
            RaycastHit hit;
            // Cast
            if (Physics.SphereCast(PlayerModel.transform.position, 1f, new Vector3(0, 0, 1), out hit))
            {
                distance = hit.distance;
            }
            if (distance > 0.2f)
            {
                constantPlayerSpeed += backwardSpeed;
            }
        }
        if (Input.GetKey(KeyCode.D))
        { // - X
            float distance = 1000f;
            RaycastHit hit;
            // Cast
            if (Physics.SphereCast(PlayerModel.transform.position, 1f, new Vector3(-1, 0, 0), out hit))
            {
                distance = hit.distance;
            }
            if (distance > 0.2f)
            {
                constantPlayerSpeed += rightSpeed;
            } else
            {
                Debug.Log("DSjhdsjah");
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            jumping = true;
        } else
        {
            jumping = false;
        }
    }

    void Start()
    {
        // Nada?
        playerRigidbody = PlayerModel.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        walled = true;
    }

    // Rigidbody y esas cosas raras
    void FixedUpdate()
    {
        checkInput();
        // Aplicamos la fuerza y desaceleramos
        constantPlayerSpeed = Vector3.Lerp(constantPlayerSpeed, Vector3.zero, deacceleration);
        constantPlayerSpeed.y = playerRigidbody.velocity.y;

        RaycastHit hit;
        float distance = 0f;

        // Miramos si tocamos el suelo
        if (Physics.SphereCast(PlayerModel.transform.position, 1f, Vector3.down, out hit))
        {
            distance = hit.distance;
        }
        

        if(distance < 0.2f)
        {
            // Tocamos el suelo?
            if (jumping)
            {
                constantPlayerSpeed.y = jumpSpeed;
            } else
            {
                constantPlayerSpeed.y = 0;
            }
            
            playerRigidbody.useGravity = false;
            
        } else
        {
            playerRigidbody.useGravity = true;
        }

        playerRigidbody.velocity = constantPlayerSpeed;

        // Mover el desto al otro
    }
}

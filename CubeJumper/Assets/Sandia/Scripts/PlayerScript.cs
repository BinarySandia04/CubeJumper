using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    public GameObject PlayerModel;
    public Transform spawnpoint;
    private Rigidbody playerRigidbody;
    public GameObject decal;
    [Tooltip("Debe de ser un GameObject vacio")]
    public GameObject decalParent;
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
    public CameraZoneTriggerManager.Orientation or = CameraZoneTriggerManager.Orientation.North;
    private CameraZoneTriggerManager.Orientation orient = CameraZoneTriggerManager.Orientation.North;
    public float airControl = 2f;
    private bool moved = false;
    Vector3 platformMomentum = Vector3.zero;

    private MovingPlatformController mpc = null;

    ColliderManager lCollider;
    ColliderManager fCollider;
    ColliderManager rCollider;
    ColliderManager bCollider;
    ColliderManager dCollider;

    public BoxCollider decalCollider;

    [SerializeField]
    private bool touchingDeco;

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
        /*
         * IMPORTANTE:
         * 
         * Para hacer sentir el cambio de orientacion más "natural", creo
         * que se deberia hacer de que se mantenga la orientacion antigua
         * hasta que la tecla del movimiento se haya despresionado. Quiero decir,
         * si estamos mirando hacia el norte y le damos a la W vamos hacia -Z, pero
         * si entramos en un cambio de orientacion y esta pasa a ser al sud, hasta que
         * la W no se despresione seguira yendo hacia -Z, y cuando se despresione y se 
         * vuelva a presionar, irá hacia +Z
         * 
         */
        if(orient == CameraZoneTriggerManager.Orientation.North)
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
        } else if(orient == CameraZoneTriggerManager.Orientation.South)
        {
            if (Input.GetKey(KeyCode.W))
            { // + Z
                constantPlayerSpeed += backwardSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            { // + X
                constantPlayerSpeed += rightSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            { // + Z
                constantPlayerSpeed += forwardSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            { // - X
                constantPlayerSpeed += leftSpeed;

            }
        } else if(orient == CameraZoneTriggerManager.Orientation.East)
        {
            if (Input.GetKey(KeyCode.W))
            { // - Z
                constantPlayerSpeed += rightSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            { // + X
                constantPlayerSpeed += forwardSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            { // + Z
                constantPlayerSpeed += leftSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            { // - X
                constantPlayerSpeed += backwardSpeed;

            }
        } else
        {
            if (Input.GetKey(KeyCode.W))
            { // - Z
                constantPlayerSpeed += leftSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            { // + X
                constantPlayerSpeed += backwardSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            { // + Z
                constantPlayerSpeed += rightSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            { // - X
                constantPlayerSpeed += forwardSpeed;

            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)) moved = true;
        else moved = false;
        
        if (Input.GetKey(KeyCode.Space) && walled && notCollidingWalls())
        {
            jumping = true;
            GetComponent<Rigidbody>().AddForce(0, jumpSpeed, 0, ForceMode.VelocityChange);
            walled = false;
        } else
        {
            jumping = false;
        }

        if (Input.GetKey(KeyCode.R))
        {
            goToSpawn();
        }

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
           orient = or;
        }
    }

    void goToSpawn()
    {
        transform.position = spawnpoint.position;
        constantPlayerSpeed = Vector3.zero;
        platformMomentum = Vector3.zero;
        playerRigidbody.velocity = Vector3.zero;
        
        foreach (Transform go in decalParent.transform)
            if (go.gameObject.activeInHierarchy)
                if (go.gameObject.name.Contains("Deco")) Destroy(go.gameObject);
    }

    void Start()
    {
        // Nada?
        currentTimeInAir = 0f;
        StartCoroutine(makeDecals());
        playerRigidbody = PlayerModel.GetComponent<Rigidbody>();
        distanciaAlSuelo = GetComponent<Collider>().bounds.extents.y;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "Propulsor")
        {
            Debug.Log("SI");
            PropulsorProperties p =  other.gameObject.GetComponent<PropulsorProperties>();
            p.playPropulsionAnimation();
            playerRigidbody.velocity = p.propulsion;
        }
        else if (other.gameObject.tag == "Muerte")
        {
           
        }
    }

    public void muerte()
    {
        goToSpawn();
        platformMomentum = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (!collision.gameObject.name.Contains("Trigger") && !collision.gameObject.name.Contains("Decal"))
        {
            if(dCollider.collided) walled = true;

            if (collision.gameObject.name == "Platform")
            {
                mpc = collision.transform.parent.GetComponent<MovingPlatformController>();
            }
            else if (collision.gameObject.tag == "Muerte"){
                goToSpawn();
                platformMomentum = Vector3.zero;
            } else if(collision.gameObject.tag == "Finish")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Muerte")
        {
            goToSpawn();
            platformMomentum = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.name.Contains("Trigger") && !collision.gameObject.name.Contains("Deco"))
        {
            if (dCollider.collided) walled = true;

            if (collision.gameObject.name == "Platform")
            {
                mpc = collision.transform.parent.GetComponent<MovingPlatformController>();
            }
            else
            {
                if (collision.gameObject.tag == "Muerte") goToSpawn();
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

    private void OnTriggerExit(Collider other)
    {

    }
    

    // Rigidbody y esas cosas raras
    void FixedUpdate()
    {
        if (isNotTouchingDeco() && walled) spawnDecal();

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
            if (walled && moved)
            {
                if(isNotTouchingDeco()) spawnDecal();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void spawnDecal()
    {
        GameObject newObject = Instantiate(decal, transform);

        if (mpc != null) newObject.transform.SetParent(mpc.transform.Find("Platform"));
        else newObject.transform.SetParent(decalParent.transform);
        Destroy(newObject, 60);
    }

    bool isNotTouchingDeco()
    {
        foreach (Transform t in decalParent.transform)
        {
            BoxCollider b = t.GetComponent<BoxCollider>();
            if (decalCollider.bounds.Intersects(b.bounds))
            {
                return false;
            }
        }
        return true; 
    }

    
}

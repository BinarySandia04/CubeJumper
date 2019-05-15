using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneTriggerManager : MonoBehaviour
{
    public bool PlayerTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.name == "Player")
        {
            PlayerTriggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.name == "Player")
        {
            PlayerTriggered = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneTriggerManager : MonoBehaviour
{
    public enum Orientation
    {
        North,
        South,
        East,
        West
    };

    public bool PlayerTriggered = false;
    [Space]
    [Header("North = -Z")]
    public Orientation orientation;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            PlayerTriggered = true;
            PlayerScript ps = other.gameObject.GetComponent<PlayerScript>();
            ps.or = orientation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            PlayerTriggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            PlayerTriggered = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public bool collided;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(!other.name.Contains("Trigger") && !other.name.Contains("Decal")) collided = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.name.Contains("Trigger") && !other.name.Contains("Decal")) collided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        collided = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPathLineDisplay : MonoBehaviour
{
    public Camera cam;
    public float lerpTimePosition;
    public float lerpTimeQuartenion;

    public Color color1;
    public Color color2;
    public Color color3;

    private void OnDrawGizmos()
    {
        Transform before = null;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CameraZoneTriggerManager>() != null) continue;
            if (before != null)
            {
                Gizmos.color = color3;
                Gizmos.DrawLine(child.position, before.position);
            }
            
            before = child;
        }
    }
}

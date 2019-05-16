using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPoints : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    private void Awake()
    {
        foreach(Transform t in transform)
        {
            points.Add(t);
        }
    }

    void OnDrawGizmos()
    {
        Transform before = null;
        foreach(Transform t in this.transform)
        {
            if(before != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(before.position, t.position);
            }
            before = t;
        }
    }
}

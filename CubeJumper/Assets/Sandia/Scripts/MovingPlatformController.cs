using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public bool Loop = true;
    public bool ReturnBackwards = true;
    public float Speed = 0.1f;
    public float Wait = 1f;
    public float initialDelay = 0f;
    public bool stopped = false;
    public Vector3 motion;

    List<Transform> points;

    GameObject platform;

    

    void Awake()
    {
        points = transform.Find("Points").GetComponent<MovingPlatformPoints>().points;
        platform = transform.Find("Platform").gameObject;
    }

    void Start()
    {
        platform.transform.position = points[0].position; // Poner la plataforma al primer punto
        StartCoroutine(platformCoroutine());
    }

    IEnumerator platformCoroutine()
    {
        yield return new WaitForSecondsRealtime(initialDelay);
        while (true)
        {
            Vector3 desiredPosition = transform.position;
            if (!ReturnBackwards)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Vector3 beforePosition = desiredPosition;
                    for (float currentLerp = 0f; currentLerp < 1; currentLerp += Speed)
                    {
                        if (i + 1 == points.Count)
                        {
                            desiredPosition = Vector3.Lerp(points[i].position, points[0].position, currentLerp);
                        }
                        else
                        {
                            desiredPosition = Vector3.Lerp(points[i].position, points[i + 1].position, currentLerp);
                        }
                        motion = desiredPosition - beforePosition;
                        platform.transform.position += motion;
                        beforePosition = desiredPosition;
                        yield return new WaitForFixedUpdate();
                    }
                    // Pon motion a zero
                    stopped = true;
                    yield return new WaitForSecondsRealtime(Wait);
                    stopped = false;
                }
            } else
            {
                
                for (int i = 0; i < points.Count - 1; i++)
                {
                    Vector3 beforePosition = desiredPosition;
                    for (float currentLerp = 0f; currentLerp < 1; currentLerp += Speed)
                    {
                        desiredPosition = Vector3.Lerp(points[i].position, points[i + 1].position, currentLerp);
                        motion = desiredPosition - beforePosition;
                        platform.transform.position += motion;
                        beforePosition = desiredPosition;
                        yield return new WaitForFixedUpdate();
                    }
                    
                }
                for(int i = points.Count - 1; i > 0; i--)
                {
                    Vector3 beforePosition = desiredPosition;
                    for (float currentLerp = 0f; currentLerp < 1; currentLerp += Speed)
                    {
                        desiredPosition = Vector3.Lerp(points[i].position, points[i - 1].position, currentLerp);
                        motion = desiredPosition - beforePosition;
                        platform.transform.position += motion;
                        beforePosition = desiredPosition;
                        yield return new WaitForFixedUpdate();
                    }
                }
                stopped = true;
                yield return new WaitForSecondsRealtime(Wait);
                stopped = false;
            }
        }
    }
}

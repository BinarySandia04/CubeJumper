using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class DeathDecalScript : MonoBehaviour
{
    private DecalProjectorComponent dp;
    public bool isDeathly = false;
    private bool deathling = false;
    public float deathlyTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        dp = GetComponent<DecalProjectorComponent>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && !isDeathly && !deathling)
        {
            StartCoroutine(transformToDeathly());
        }
    }

    IEnumerator transformToDeathly()
    {
        Color initial = Color.white;
        Color final = Color.red;
        deathling = true;
        for(float i = 0; i < deathlyTime; i += Time.deltaTime)
        {
            float lerping = i / deathlyTime;
            Material c = dp.m_Material;
            c.SetColor("_BaseColor", Color.Lerp(initial, final, lerping));
            dp.m_Material = c;
            yield return new WaitForEndOfFrame();
        }
        isDeathly = true;
        transform.parent.gameObject.tag = "Muerte";
        yield return null;
    }
}

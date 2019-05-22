using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CronometerUI : MonoBehaviour
{
    TextMeshProUGUI text;
    float time = 0;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(cronometer());
    }

    IEnumerator cronometer()
    {
        while (true)
        {
            time += Time.deltaTime;

            int milis = (int) ((time * 100) % 100);
            int seconds = (int) ((time) % 60);
            int minutes = (int)((time) / 60);
            // minutes:seconds:milis

            string mils = milis + "";
            if (mils.Length == 1) mils = "0" + mils;
            string mins = minutes + "";
            if (mins.Length == 1) mins = "0" + mins;
            string secs = seconds + "";
            if (secs.Length == 1) secs = "0" + secs;

            text.text = mins + ":" + secs + ":" + mils;

            if (Input.GetKey(KeyCode.R))
            {
                time = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

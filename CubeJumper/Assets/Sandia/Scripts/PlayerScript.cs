using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	void Update ()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(0.001f, 0, 0));
        }
    }
}

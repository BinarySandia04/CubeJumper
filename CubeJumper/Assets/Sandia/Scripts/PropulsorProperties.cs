using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsorProperties : MonoBehaviour
{
    public Vector3 propulsion = new Vector3();

    private Animation muelle = null;

    public void playPropulsionAnimation()
    {
        if (muelle == null) muelle = transform.GetChild(0).GetComponent<Animation>();
        muelle.Play();
    }
}

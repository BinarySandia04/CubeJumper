using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPathPoint : MonoBehaviour
{
    public CameraZoneTriggerManager trigger;
    public Camera cam;
    public float lerptimepos, lerptimeqrt;
    bool triggered = false;

    Color color1, color2;

    private void Awake()
    {
        trigger = transform.parent.Find("Trigger" + gameObject.name).GetComponent<CameraZoneTriggerManager>();

        CameraPathLineDisplay temp = transform.GetComponentInParent<CameraPathLineDisplay>();
        cam = temp.cam;
        lerptimepos = temp.lerpTimePosition;
        lerptimeqrt = temp.lerpTimeQuartenion;
    }

    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        CameraPathLineDisplay temp = transform.GetComponentInParent<CameraPathLineDisplay>();
        cam = temp.cam;
        lerptimepos = temp.lerpTimePosition;
        lerptimeqrt = temp.lerpTimeQuartenion;
        color1 = temp.color1;
        color2 = temp.color2;

        Gizmos.color = color1;
        Gizmos.DrawCube(transform.position, Vector3.one / 2f);
        Gizmos.color = color2;
        Gizmos.DrawRay(transform.position, transform.rotation * Vector3.forward);
    }

    private void FixedUpdate()
    {
        if (trigger.PlayerTriggered && !triggered)
        {
            StartCoroutine(doTheLerping());
        }
        if (Input.GetKey(KeyCode.R)) triggered = false;
    }

    IEnumerator doTheLerping()
    {
        triggered = true;
        for(int i = 0; i < 500; i++)
        {
            Vector3 cameraPosition = cam.transform.position;
            Quaternion cameraQuaternion = cam.transform.rotation;

            Vector3 desiredPosition = Vector3.Lerp(cameraPosition, transform.position, lerptimepos);
            Quaternion desiredQuartenion = Quaternion.Lerp(cameraQuaternion, transform.rotation, lerptimeqrt);

            cam.transform.position = desiredPosition;
            cam.transform.rotation = desiredQuartenion;
            yield return new WaitForFixedUpdate();
        }
        triggered = false;
        yield return null;
    }
}

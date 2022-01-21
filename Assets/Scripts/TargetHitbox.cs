using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitbox : MonoBehaviour
{
    public GameObject mainTarget;
    public GameObject playerMesh;
    private List<GameObject> targetsInView = new List<GameObject>();
    private float mainTargetDistence = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Target")
        {
            targetsInView.Add(collision.gameObject);
            float distance = Vector3.Distance(playerMesh.transform.position, collision.gameObject.transform.position);
            if (distance > mainTargetDistence)
            {
                mainTarget = collision.gameObject;
                mainTargetDistence = distance;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            targetsInView.Remove(collision.gameObject);
            if (targetsInView.Count == 0)
            {
                mainTarget = null;
                mainTargetDistence = 0f;
            }
        }
    }

    private void Update()
    {
        // Debug.Log(targetsInView.Count);
    }
}

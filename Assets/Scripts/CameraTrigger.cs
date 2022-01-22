using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPosition
{
    Rear,
    Side
}

public class CameraTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cameraPoint;
    public CameraPosition cameraPosition;

    void Start()
    {
        // Debug.Log("Test");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

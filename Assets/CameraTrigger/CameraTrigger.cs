using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPosition
{
    Rear,
    Front,
    Left,
    Right
}

public class CameraTrigger : MonoBehaviour
{
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

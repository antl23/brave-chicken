using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// CameraController is responsible for handling the third person camera that follows the player
// It allows rotation based on mouse input
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distance = 5.0f; //offset from player
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private float minY = -20f;
    [SerializeField] private float maxY = 60f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector3 currentVelocity; //to smooth the camera movement

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("yo where the player at");
            return;
        }
        //initialize the camera position with the current values
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;

        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (player == null) return; //just so if the player dies or disappears, the game doesnt break

        //get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX += mouseX;
        rotationY -= mouseY;

        //clamp the rotation
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        //new camera postion and rotation
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        Vector3 offset = rotation * new Vector3(0, 3f, -distance);
        Vector3 targetPosition = player.position + offset;
        //smooth the camera movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.1f);
        //makes the camera look slightly above the player
        transform.LookAt(player.position + Vector3.up * 3);
    }
}

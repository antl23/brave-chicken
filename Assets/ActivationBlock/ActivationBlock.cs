using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationBlock : MonoBehaviour
{
    public GameObject inactiveObject;
    public GameObject activeObject;
    public Transform objectToMove;
    public Transform moveTarget;
    public Color activeColor;
    public float moveSpeed;
    public bool isActive;
    public Material objectMaterial;
    private Vector3 startPos;
    private bool returning;
    private Color startColor;


    // Start is called before the first frame update
    void Start()
    {
        if (!isActive) Deactivate();
        startPos = objectToMove.position;
        Debug.Log(objectToMove.position + " " + moveTarget.position);
        startColor = objectMaterial.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive) {
/*            if (returning)
            {
                objectToMove.position = Vector3.MoveTowards(objectToMove.position, startPos, moveSpeed);
            }
            else
            {
                objectToMove.position = Vector3.MoveTowards(objectToMove.position, moveTarget.position, moveSpeed);
            }*/
            objectToMove.position = !returning
                ? Vector3.MoveTowards(objectToMove.position, moveTarget.position, moveSpeed)
                : Vector3.MoveTowards(objectToMove.position, startPos, moveSpeed);
            if (objectToMove.position == startPos)
            {
                //Debug.Log("At start");
                returning = false;
                Deactivate();
            }
            if (objectToMove.position == moveTarget.position)
            {
                //Debug.Log("End");
                returning = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Egg" && !isActive)
        {
            Activate();
            GetComponent<AudioSource>().Play();
        }
    }

    private void Activate()
    {
        GetComponent<AudioSource>().Play();
        isActive = true;
        activeObject.SetActive(true);
        inactiveObject.SetActive(false);
        objectMaterial.color = activeColor;
    }

    private void Deactivate()
    {
        GetComponent<AudioSource>().Play();
        isActive = false;
        activeObject.SetActive(false);
        inactiveObject.SetActive(true);
        objectMaterial.color = startColor;
    }
}

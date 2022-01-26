using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationBlock : MonoBehaviour
{
    public GameObject inactiveObject;
    public GameObject activeObject;
    public Transform startPosition;
    public GameObject platform;
    public Transform moveTarget;
    public Color startColor;
    public Color activeColor;
    public Color secondaryStartColor;
    public Color secondaryActiveColor;
    public float moveSpeed;
    public bool isActive;
    private bool returning;
    private GameObject objectToMove;

    void Start()
    {
        objectToMove = Instantiate(platform, startPosition.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive) {
            objectToMove.transform.position = !returning
                ? Vector3.MoveTowards(objectToMove.transform.position, moveTarget.position, moveSpeed)
                : Vector3.MoveTowards(objectToMove.transform.position, startPosition.position, moveSpeed);
            if (objectToMove.transform.position == startPosition.position)
            {
                returning = false;
                Deactivate();
            }
            if (objectToMove.transform.position == moveTarget.position)
            {
                returning = true;
                GetComponent<AudioSource>().Play();
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
        Material[] materials = objectToMove.GetComponent<Renderer>().materials;
        materials[0].color = activeColor;
        materials[1].color = secondaryActiveColor;
    }

    private void Deactivate()
    {
        GetComponent<AudioSource>().Play();
        isActive = false;
        activeObject.SetActive(false);
        inactiveObject.SetActive(true);
        Material[] materials = objectToMove.GetComponent<Renderer>().materials;
        materials[0].color = startColor;
        materials[1].color = secondaryStartColor;
    }
}

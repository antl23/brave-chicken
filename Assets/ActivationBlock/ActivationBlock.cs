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
    //public Material objectMaterial;
    // public GameObject objectMaterial;
    private bool returning;
    private GameObject objectToMove;
    // private Color startColor;


    // Start is called before the first frame update
    void Start()
    {
        // if (!isActive) Deactivate();
        // Debug.Log(objectToMove.transform.position + " " + moveTarget.position);
        // startColor = objectMaterial.color;
        // startColor = Color.gray; // objectMaterial.color;
        // objectMaterial.GetComponent<Renderer>().material.color = startColor;
        //objectMaterial.color = startColor;
        objectToMove = Instantiate(platform, startPosition.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive) {
/*            if (returning)
            {
                objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, startPos, moveSpeed);
            }
            else
            {
                objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, moveTarget.position, moveSpeed);
            }*/
            objectToMove.transform.position = !returning
                ? Vector3.MoveTowards(objectToMove.transform.position, moveTarget.position, moveSpeed)
                : Vector3.MoveTowards(objectToMove.transform.position, startPosition.position, moveSpeed);
            if (objectToMove.transform.position == startPosition.position)
            {
                //Debug.Log("At start");
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
        // Debug.Log(other.tag);
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
        //objectMaterial.color = activeColor;
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
        //objectMaterial.color = startColor;
        Material[] materials = objectToMove.GetComponent<Renderer>().materials;
        materials[0].color = startColor;
        materials[1].color = secondaryStartColor;
    }
}

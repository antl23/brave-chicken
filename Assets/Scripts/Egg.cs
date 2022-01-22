using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("egg");
    }

    public void Break()
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        if (target != null)
        {

        }
    }
}

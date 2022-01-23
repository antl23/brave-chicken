using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public void Break()
    {
        // doesn't work
        GetComponent<AudioSource>().Play();
        Invoke("End", 1);
        
    }

    void End()
    {
        Destroy(gameObject);
    }

}

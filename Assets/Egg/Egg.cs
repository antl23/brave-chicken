using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public void Break()
    {
        // doesn't work
        Debug.Log(GetComponent<AudioSource>().clip);
        GetComponent<AudioSource>().Play();
        // End();
        Invoke("End", 5);
    }

    void End()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            transform.Rotate(Vector3.right, 1f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{

    public ParticleSystem splatter;

    private float timer;

    public void Start()
    {
        timer = 1;
    }

    public void Break()
    {
        GetComponent<AudioSource>().Play();
        splatter.Play();
        Invoke("End", .5f);
    }

    public void End()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            transform.Rotate(Vector3.right, 1f);
        }
        timer -= Time.deltaTime;
        Debug.Log(timer);
        if (timer <= 0)
        {
            End();
        }
    }

}

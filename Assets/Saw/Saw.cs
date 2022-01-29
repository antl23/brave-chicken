using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float turnRate;
    public float moveRate;
    public Transform start;
    public Transform end;
    public bool returning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, returning ? start.position : end.position, moveRate);
            if (transform.position == start.position)
            {
                returning = false;
                GetComponents<AudioSource>()[1].Play();
            }
            else if (transform.position == end.position)
            {
                returning = true;
                GetComponents<AudioSource>()[1].Play();
            }
            transform.Rotate(Vector3.up, turnRate); 
        }
    }
}

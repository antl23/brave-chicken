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
        if (Time.timeScale == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, returning ? start.position : end.position, moveRate);
            if (transform.position == start.position)
            {
                returning = false;
                GetComponent<AudioSource>().Play();
            }
            else if (transform.position == end.position)
            {
                returning = true;
                GetComponent<AudioSource>().Play();
            }
            transform.Rotate(Vector3.up, turnRate); 
        }
    }
}

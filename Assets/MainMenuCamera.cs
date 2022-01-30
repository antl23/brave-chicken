using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuCamera : MonoBehaviour
{
    public float moveRate;
    public List<Transform> points;
    private Transform nextPoint;

    // Start is called before the first frame update
    void Start()
    {
        nextPoint = points[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPoint.position, moveRate);
            for (int i = 0; i < points.Count; i++)
            {
                if (transform.position == points[i].position)
                {
                    if (i == points.Count - 1)
                    {
                        nextPoint = points[0];
                    }
                    else
                    {
                        nextPoint = points[i + 1];
                    }
                }
            }
        }
    }
}

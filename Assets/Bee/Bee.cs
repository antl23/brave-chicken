using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public float moveRate;
    public List<Transform> points;
    private Transform nextPoint;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        nextPoint = points[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale == 1)
        {
            if (!dead)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPoint.position, moveRate);
                for (int i = 0; i < points.Count; i++)
                {
                    if (transform.position == points[i].position)
                    {
                        if (i == points.Count - 1)
                        {
                            nextPoint = points[0];
                        } else
                        {
                            nextPoint = points[i + 1];
                        }
                    }
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.tag);
        if (other.tag == "Egg")
        {
            dead = true;
            var clips = GetComponents<AudioSource>();
            clips[0].Stop(); // stop buzzing
            clips[1].Play(); // play death noise
            GetComponent<Animator>().SetTrigger("Death");
            gameObject.tag = "Untagged"; // wont hurt player
            Invoke("End", 2);
            // GetComponent<AudioSource>().Play();
        }
    }

    private void End() {
        Destroy(gameObject.transform.parent.gameObject);
    }
}

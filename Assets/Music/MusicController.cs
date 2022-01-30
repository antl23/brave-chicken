using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public Player player;
    public AudioSource gameMusic;
    public AudioSource deathMusic;
    public AudioSource victoryMusic;
    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            if (player.health == 0)
            {
                finished = true;
                gameMusic.Stop();
                deathMusic.Play();
            } else if (player.finished)
            {
                finished = true;
                gameMusic.Stop();
                victoryMusic.Play();
            }
        }
    }
}

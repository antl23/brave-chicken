using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefabToSpawn;
    public GameObject existingObject;
    public uint spawnDelay = 360;
    private uint spawnTimer = 0;
    private bool justSpawned = true;

    // Update is called once per frame
    void Update()
    {
        if (existingObject == null)
        {
            if (spawnTimer == 0 && !justSpawned)
            {
                existingObject = Instantiate(prefabToSpawn, transform);
                justSpawned = true;
            } else 
            {
                spawnTimer--;
            }
        } else if (justSpawned)
        {
            spawnTimer = spawnDelay;
            justSpawned = false;
        }
    }
}

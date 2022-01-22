using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitbox : MonoBehaviour
{
    public GameObject playerMesh;
    public GameObject particleSys;
    public Egg projectile;
    public float projectileSpeed;
    public float heighOffset;
    private GameObject mainTarget;
    private GameObject currentParticleSys;
    private GameObject currProjectile;
    private List<GameObject> targetsInView = new List<GameObject>();
    private float mainTargetDistence = 0f;
    private GameObject currTarget;
    

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Target")
        {
            bool targetChanged = false;
            targetsInView.Add(collider.gameObject);
            float distance = Vector3.Distance(playerMesh.transform.position, collider.gameObject.transform.position);
            Debug.DrawRay(playerMesh.transform.position, (collider.transform.position - playerMesh.transform.position), Color.green);
            Debug.Log(distance + "   " + mainTargetDistence);
            if (distance > mainTargetDistence)
            {
                targetChanged = true;
                mainTarget = collider.gameObject;
                mainTargetDistence = distance;
            }
           /* RaycastHit[] hits;
            hits = Physics.RaycastAll(playerMesh.transform.position, (collider.transform.position - playerMesh.transform.position), Mathf.Infinity);
            */if (targetChanged)
            {
                Destroy(currentParticleSys);
                currentParticleSys = Instantiate(particleSys, mainTarget.transform);
                currentParticleSys.transform.LookAt(playerMesh.transform);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Target")
        {
            targetsInView.Remove(collider.gameObject);
            if (collider.gameObject == mainTarget)
            {
                mainTarget = null;
                mainTargetDistence = 0f;
                Destroy(currentParticleSys);
                currentParticleSys = null;
            }
        }
    }

    private void Update()
    {
       if (Input.GetButtonDown("Activate") && currProjectile == null)
       {
            Vector3 position = playerMesh.transform.position;
            Vector3 point = new Vector3(position.x, position.y + heighOffset, position.z);
            currProjectile = Instantiate(projectile.gameObject, point, playerMesh.transform.rotation);
            currTarget = mainTarget;
       }
       if (currProjectile)
       {
            float step = projectileSpeed * Time.deltaTime;
            currProjectile.transform.position = Vector3.MoveTowards(currProjectile.transform.position, currTarget.transform.position, step);
            if (currProjectile.transform.position == currTarget.transform.position) {
                Destroy(currProjectile);
                currProjectile = null;
                currTarget = null;
            }        
        }
    }
}

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
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Target")
        {
            bool targetChanged = false;
            targetsInView.Add(collider.gameObject);
            float distance = Vector3.Distance(playerMesh.transform.position, collider.gameObject.transform.position);
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
                RaycastHit hit;
                Debug.DrawRay(playerMesh.transform.position, (mainTarget.transform.position - playerMesh.transform.position), Color.red, 5);
                if (Physics.Raycast(playerMesh.transform.position, mainTarget.transform.position, out hit, Mathf.Infinity))
                {

                    Debug.Log(mainTarget.transform.position + " vs " + hit.transform.position);
                    Debug.DrawRay(playerMesh.transform.position, (hit.transform.position - playerMesh.transform.position), Color.red, 5);
                    // doesn't work (wrong spot)
                    currentParticleSys = Instantiate(particleSys, hit.transform.position, new Quaternion());
                    currentParticleSys.transform.LookAt(new Vector3(playerMesh.transform.position.x, playerMesh.transform.position.y + 0.5f, playerMesh.transform.position.z));
                }
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
       if (Input.GetButtonDown("Activate") && currProjectile == null && mainTarget != null)
       {
            Vector3 position = playerMesh.transform.position;
            Vector3 point = new Vector3(position.x, position.y + heighOffset, position.z);
            currProjectile = Instantiate(projectile.gameObject, point, playerMesh.transform.rotation);
            currTarget = mainTarget;
            GetComponent<AudioSource>().Play();
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

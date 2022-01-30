using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitbox : MonoBehaviour
{
    // public GameObject playerMesh;
    public GameObject particleSys;
    public Transform armTransform;
    public Egg projectile;
    public float projectileSpeed;
    public float heighOffset;
    private GameObject mainTarget;
    private GameObject currentParticleSys;
    private Egg currProjectile;
    private List<GameObject> targetsInView = new List<GameObject>();
    private float mainTargetDistence = Mathf.Infinity;
    private GameObject currTarget;
    

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            bool targetChanged = false;
            targetsInView.Add(collider.gameObject);
            float distance = Vector3.Distance(armTransform.position, collider.gameObject.transform.position);
            if (distance < mainTargetDistence)
            {
                targetChanged = true;
                mainTarget = collider.gameObject;
                mainTargetDistence = distance;
            }
            if (targetChanged)
            {
                Destroy(currentParticleSys);
                RaycastHit hit;
                if (Physics.Raycast(armTransform.position, (mainTarget.transform.position - armTransform.position), out hit, Mathf.Infinity, LayerMask.GetMask("Target")))
                {
                    currentParticleSys = Instantiate(particleSys, hit.point, new Quaternion(), mainTarget.transform);
                    currentParticleSys.transform.LookAt(new Vector3(armTransform.position.x, armTransform.position.y, armTransform.position.z));
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        RaycastHit hit;
        if (collider != null && collider.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            targetsInView.Remove(collider.gameObject);
            mainTarget = targetsInView.Count > 0 ? targetsInView[0] : null;
            if (mainTarget == null)
            {
                mainTargetDistence = Mathf.Infinity;
                Destroy(currentParticleSys);
                currentParticleSys = null;
            } else if (Physics.Raycast(armTransform.position, (mainTarget.transform.position - armTransform.position), out hit, Mathf.Infinity, LayerMask.GetMask("Target")))
            {
                Destroy(currentParticleSys);
                currentParticleSys = Instantiate(particleSys, hit.point, new Quaternion(), mainTarget.transform);
                mainTargetDistence = hit.distance;
            }
            
        }
    }

    private void Update()
    {
       if (Input.GetButtonDown("Activate") && currProjectile == null && mainTarget != null)
       {
            Vector3 position = armTransform.position;
            Vector3 point = new Vector3(position.x, position.y + heighOffset, position.z);
            currProjectile = Instantiate(projectile, point, armTransform.rotation);
            currTarget = mainTarget;
            GetComponent<AudioSource>().Play();
       }
       if (currProjectile != null && currTarget != null)
       {
            float step = projectileSpeed * Time.deltaTime;
            currProjectile.transform.position = Vector3.MoveTowards(currProjectile.transform.position, currTarget.transform.position, step);
            if (currProjectile.transform.position == currTarget.transform.position) {
                currProjectile.Break();
                currProjectile = null;
                currTarget = null;
            }        
        }
        if (currentParticleSys != null)
        {
            currentParticleSys.transform.LookAt(new Vector3(armTransform.position.x, armTransform.position.y, armTransform.position.z));
        }
    }
}

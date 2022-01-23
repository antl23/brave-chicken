using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float dashMultiplier = 3.0f;
    public uint dashLength = 120;
    public uint dashDelay = 120;
    public float gravity = 20.0f;
    public Transform cameraTransform;
    public Transform meshTransform;
    public Animator playerAnimator;
    public Transform rearCameraPoint;
    public Transform frontCameraPoint;
    public Transform leftCameraPoint;
    public Transform rightCameraPoint;
    public Transform cameraCenter;
    public CameraPosition cameraPosition = CameraPosition.Rear;
    public float lookSpeed = 20.0f;
    public float lookXLimit = 60.0f;
    private bool canDoubleJump = true;
    private bool canDash = true;
    private uint dashTimer = 0;
    private bool sideScroll = false;
    // private bool inTrigger = false;
    private float saveZ;
    private bool warp = false;
    private CameraTrigger lastTrigger;
    private CameraPosition camPos = CameraPosition.Rear;
    private Vector3? cameraTarget;
    private bool rotating = false;
    private Vector3 startCamPos;
    private float cameraMoveAmount = 0.0f;

    public bool canMove = true;

    CharacterController characterController;
    Vector3 direction = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        // frontCameraPoint;
        // cameraTarget = frontCameraPoint.position;
        // StartCoroutine(rotateObject(cameraTransform, cameraTarget.rotation.eulerAngles, 3f));
        cameraTransform.LookAt(cameraCenter);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Boost":
                canDoubleJump = true;
                canDash = true;
                Destroy(other.gameObject);
                break;
            case "Camera":
                CameraPosition camPos = other.GetComponent<CameraTrigger>().cameraPosition;
                switch (camPos)
                {
                    case CameraPosition.Rear:
                        cameraTarget = rearCameraPoint.localPosition;
                        startCamPos = cameraTransform.localPosition;
                        cameraMoveAmount = 0.0f;
                        // cameraTransform.SetParent(rearCameraPoint, false);
                        break;
                    case CameraPosition.Front:
                        cameraTarget = frontCameraPoint.localPosition;
                        startCamPos = cameraTransform.localPosition;
                        cameraMoveAmount = 0.0f;
                        break;
                    case CameraPosition.Left:
                        cameraTarget = leftCameraPoint.localPosition;
                        startCamPos = cameraTransform.localPosition;
                        cameraMoveAmount = 0.0f;
                        break;
                    case CameraPosition.Right:
                        cameraTarget = rightCameraPoint.localPosition;
                        startCamPos = cameraTransform.localPosition;
                        cameraMoveAmount = 0.0f;
                        break;
                }
                break;
            case "Spring":
                other.gameObject.GetComponent<AudioSource>().Play();
                direction.y = jumpSpeed * 2;
                Animator animator = other.gameObject.GetComponent<Animator>();
                animator.SetTrigger("Bounce");
                break;
            case "Spikes":
                other.gameObject.GetComponent<AudioSource>().Play();
                other.gameObject.GetComponent<Animator>().SetTrigger("Activate");
                TakeDamage();
                break;
            case "Saw":
                TakeDamage();
                break;
        }
/*        // Debug.Log(other.tag);
        if (other.tag == "Boost") {
            canDoubleJump = true;
            canDash = true;
            Destroy(other.gameObject);
        }
        CameraTrigger trigger = other.GetComponent<CameraTrigger>();
        if (other.tag == "Sidescroll" && !inTrigger && trigger != lastTrigger) 
        {
            inTrigger = true;
            GameObject cameraPoint = trigger.cameraPoint;
            lastTrigger = trigger;
            camPos = trigger.cameraPosition;
            Debug.Log("Enter");
            characterController.enabled = false;
            characterController.transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
            characterController.enabled = true;
            *//*if (camPos == CameraPosition.Side) {
                sideScroll = true;
                Debug.Log("Side");
                cameraTarget = cameraPoint.transform.position;
                //cameraTransform.position = cameraPoint.transform.position;
                //cameraTransform.rotation = Quaternion.LookRotation(new Vector3(transform.position.x, 0, transform.position.z));
                //cameraTransform.LookAt(transform.position);
                cameraTransform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            } else
            {
                Debug.Log("Rear");
                cameraTarget = cameraPoint.transform.position;
                cameraTransform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                sideScroll = false;
            }*//*
        }
        if (other.tag == "Spring")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            direction.y = jumpSpeed * 2;
            Animator animator = other.gameObject.GetComponent<Animator>();
            Debug.Log(animator.GetParameter(0));
            animator.SetTrigger("Bounce");
            Debug.Log("Bounce");
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log(other.tag);
        if (other.tag == "Sidescroll")
        {
            // inTrigger = false;
          /*  characterController.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            characterController.enabled = true;*/
            // sideScroll = false;
        }
    }

    void Update()
    {
        if (!characterController.isGrounded) {
            playerAnimator.SetBool("InAir", true);
        } else
        {
            playerAnimator.SetBool("InAir", false);
        }
        if (canMove)
        {
            Vector3 forward = new Vector3(cameraTransform.transform.forward.x, 0, cameraTransform.transform.forward.z);
            Vector3 right = new Vector3(cameraTransform.transform.right.x, 0, cameraTransform.transform.right.z);
            float curSpeedX = sideScroll ? 0 : speed * Input.GetAxis("Vertical");
            float curSpeedZ = speed * Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Fire3") && canDash)
            {
                dashTimer = dashLength + dashDelay;
                canDash = false;
            }
            if (dashTimer > 0) {
                dashTimer--;
                if (dashTimer > dashDelay) { 
                    curSpeedZ *= dashMultiplier;
                    curSpeedX *= dashMultiplier;
                }
            }
            float curSpeedY = dashTimer == 0 ? direction.y : 0;
            direction = (forward * curSpeedX) + (right * curSpeedZ) + (Vector3.up * curSpeedY);
            if (characterController.isGrounded)
            {
                canDoubleJump = true;
                canDash = true;
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (canDoubleJump && !characterController.isGrounded)
                {
                    canDoubleJump = false;
                    direction.y = jumpSpeed;
                }
                else if (characterController.isGrounded) { 
                    direction.y = jumpSpeed;
                }
            }

       

            if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
            {
                playerAnimator.SetBool("Walk", true);
                meshTransform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                // meshTransform.rotation *= Quaternion.AngleAxis(180, meshTransform.up);
            }
            else {
                playerAnimator.SetBool("Walk", false);
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (dashTimer == 0) {
                direction.y -= gravity * Time.deltaTime;
            }
            characterController.Move(direction * Time.deltaTime);
            // Move the camera
            // rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            // rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            // rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            // playerCameraParent.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        }
        if (cameraTarget != null)
        {
            // float step = Mathf.Range;//lookSpeed * Time.deltaTime;
           /* Vector3 newPos = Vector3.MoveTowards(cameraTransform.position, cameraTarget.Value, step);
            if (cameraTransform.position == newPos)
            {
                cameraTarget = null;
                return;
            }*/
            cameraTransform.localPosition = Vector3.Lerp(startCamPos, cameraTarget.Value, cameraMoveAmount);
            cameraMoveAmount += Time.deltaTime * lookSpeed;
            cameraMoveAmount = Mathf.Clamp(cameraMoveAmount, 0.0f, 1.0f);
            cameraTransform.LookAt(cameraCenter);
            // cameraTransform.rotation = Quaternion.Lerp()
            if (cameraMoveAmount == 1)
            {
                cameraTransform.localPosition = cameraTarget.Value;
                cameraMoveAmount = 0.0f;
                cameraTarget = null;
            }
        }
    }

    IEnumerator rotateObject(Transform gameObjectToMove, Vector3 eulerAngles, float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Vector3 newRot = gameObjectToMove.eulerAngles + eulerAngles;

        Vector3 currentRot = gameObjectToMove.eulerAngles;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObjectToMove.eulerAngles = Vector3.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }

    void TakeDamage()
    {
        GetComponent<AudioSource>().Play();
    }
}
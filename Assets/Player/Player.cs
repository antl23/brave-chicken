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
    public GameObject shadowObject;
    public float lookSpeed = 20.0f;
    public float lookXLimit = 60.0f;
    public uint health;
    public uint maxIFrames;
    public GameObject deathMenu;
    public GameObject victoryMenu;
    public ParticleSystem smokeSys;
    public ParticleSystem bloodSys;
    public GameObject materialObject;
    public bool finished;
    private uint iframes;
    private bool canDoubleJump = true;
    private bool canDash = true;
    private uint dashTimer = 0;
    private bool sideScroll = false;
    private Vector3? cameraTarget;
    private Vector3 startCamPos;
    private float cameraMoveAmount = 0.0f;
    private GameObject shadow;
    private Vector3 initialShadowScale;
    private Color initColor;
    private Vector3 lastGroundedPoint;
    private AudioSource walkSound;

    public bool canMove = true;

    CharacterController characterController;
    Vector3 direction = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        cameraTransform.LookAt(cameraCenter);
        shadow = Instantiate(shadowObject, transform);
        initialShadowScale = shadow.transform.localScale;
        initColor = materialObject.GetComponent<Renderer>().material.GetColor("RimColor");
        //Debug.Log(materialObject.GetComponent<Renderer>().material.GetColor("RimColor"));
        walkSound = GetComponents<AudioSource>()[3];
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Platform")
        {
            transform.position = col.gameObject.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Platform":
                transform.SetParent(other.transform);
                break;
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
                dashTimer = 0;
                canDoubleJump = true;
                canDash = true;
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
            case "KillBox":
                TakeDamage();
                if (health > 0)
                {
                    direction = Vector3.zero;
                    characterController.enabled = false;
                    transform.position = new Vector3(lastGroundedPoint.x, lastGroundedPoint.y + 1, lastGroundedPoint.z);
                    characterController.enabled = true;
                }
                break;
            case "Victory":
                finished = true;
                Time.timeScale = 0.1f;
                victoryMenu.SetActive(true);
                canMove = false;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            transform.SetParent(null);
        }
    }

    private void FixedUpdate()
    {
        if (canMove && dashTimer > 0)
        {
            dashTimer--;
        }
        if (iframes > 0)
        {
            iframes--;
        }
        else if (health > 0)
        {
            materialObject.GetComponent<Renderer>().material.SetColor("RimColor", initColor);
        }
    }

    void Update()
    {
        if (!characterController.isGrounded) {
            playerAnimator.SetBool("InAir", true);
            GetComponents<AudioSource>()[3].Stop();
        }
        else
        {
            playerAnimator.SetBool("InAir", false);
            if (transform.parent == null) lastGroundedPoint = transform.position;
        }
        if (canMove)
        {
            Vector3 forward = new Vector3(cameraTransform.transform.forward.x, 0, cameraTransform.transform.forward.z);
            Vector3 right = new Vector3(cameraTransform.transform.right.x, 0, cameraTransform.transform.right.z);
            float curSpeedX = sideScroll ? 0 : speed * Input.GetAxis("Vertical");
            float curSpeedZ = speed * Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Fire3") && canDash && (curSpeedZ != 0 || curSpeedX != 0))
            {
                dashTimer = dashLength + dashDelay;
                canDash = false;
                smokeSys.Play();
                GetComponents<AudioSource>()[1].Play();
            }
            if (dashTimer > 0) {
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
                direction.y = 0;
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (canDoubleJump && !characterController.isGrounded)
                {
                    canDoubleJump = false;
                    direction.y = jumpSpeed;
                    GetComponents<AudioSource>()[2].Play();
                }
                else if (characterController.isGrounded) { 
                    direction.y = jumpSpeed;
                    GetComponents<AudioSource>()[2].Play();
                }
            }

       

            if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
            {
                playerAnimator.SetBool("Walk", true);
                if (!walkSound.isPlaying) walkSound.PlayDelayed(.5f);
                meshTransform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            }
            else {
                playerAnimator.SetBool("Walk", false);
                GetComponents<AudioSource>()[3].Stop();
            }

            if (dashTimer == 0) {
                direction.y -= gravity * Time.deltaTime;
            }
            if (iframes > maxIFrames - 3)
            {
                direction.x = direction.x * -5;
                direction.z = direction.z * -5;
            }

            characterController.Move(direction * Time.deltaTime);
        }
        if (cameraTarget != null)
        {
            cameraTransform.localPosition = Vector3.Lerp(startCamPos, cameraTarget.Value, cameraMoveAmount);
            cameraMoveAmount += Time.deltaTime * lookSpeed;
            cameraMoveAmount = Mathf.Clamp(cameraMoveAmount, 0.0f, 1.0f);
            cameraTransform.LookAt(cameraCenter);
            if (cameraMoveAmount == 1)
            {
                cameraTransform.localPosition = cameraTarget.Value;
                cameraMoveAmount = 0.0f;
                cameraTarget = null;
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            shadow.transform.position = hit.point;
            float scaleFactor = Mathf.Clamp(10 / (hit.distance + 10), 0.2f, 1);
            shadow.transform.localScale =
                 new Vector3(
                     initialShadowScale.x * scaleFactor,
                     initialShadowScale.y,
                     initialShadowScale.z * scaleFactor
                 );
        } else
        {
            shadow.transform.position = Vector3.zero;
        }
        if (Time.timeScale < 1)
        {
            walkSound.Stop();
        }
    }

    void TakeDamage()
    {
        if (iframes == 0)
        {
            health--;
            GetComponent<AudioSource>().Play();
            if (health == 0)
            {
                Time.timeScale = 0.1f;
                canMove = false;
                Invoke("ShowDeathMenu", 0.3f);
            }
            materialObject.GetComponent<Renderer>().material.SetColor("RimColor", Color.red);
            iframes = maxIFrames;
            bloodSys.Play();
        }
    }

    void ShowDeathMenu()
    {
        finished = true;
        deathMenu.SetActive(true);
    }
}
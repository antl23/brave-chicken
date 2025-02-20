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
    public Transform meshTransform;
    public Animator playerAnimator;
    public GameObject shadowObject;
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
        shadow = Instantiate(shadowObject, transform);
        initialShadowScale = shadow.transform.localScale;
        initColor = materialObject.GetComponent<Renderer>().material.GetColor("RimColor");
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
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            float moveZ = Input.GetAxis("Horizontal") * speed;
            float moveX = Input.GetAxis("Vertical") * speed;
            direction = (forward * moveX) + (right * moveZ) + Vector3.up * direction.y;

            if (Input.GetButtonDown("Fire3") && canDash && (moveX != 0 || moveZ != 0))
            {
                dashTimer = dashLength + dashDelay;
                canDash = false;
                smokeSys.Play();
                GetComponents<AudioSource>()[1].Play();
            }
            if (dashTimer > 0) {
                if (dashTimer > dashDelay) { 
                    moveZ *= dashMultiplier;
                    moveX *= dashMultiplier;
                }
            }
            float curSpeedY = dashTimer == 0 ? direction.y : 0;
            direction = (forward * moveX) + (right * moveZ) + (Vector3.up * curSpeedY);
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
                if (direction.x != 0 || direction.z != 0)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                }

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
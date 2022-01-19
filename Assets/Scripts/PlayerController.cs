using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float dashMultiplier = 3.0f;
    public uint dashLength = 120;
    public uint dashDelay = 120;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public Transform meshTransform;
    public Animator playerAnimator;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    private bool canDoubleJump = true;
    private bool canDash = true;
    private uint dashTimer = 0;

    CharacterController characterController;
    Vector3 direction = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    void Update()
    {
        Vector3 forward = new Vector3(playerCameraParent.transform.forward.x, 0, playerCameraParent.transform.forward.z);
        Vector3 right = new Vector3(playerCameraParent.transform.right.x, 0, playerCameraParent.transform.right.z);
        float curSpeedX = speed * Input.GetAxis("Vertical");
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
            meshTransform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(direction.x, 0, direction.z));
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
        // Move the controller
        rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
        playerCameraParent.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
    }
}
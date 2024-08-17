using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintAddition;
    float currentSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    private float originalSpeed, originalAddition, originalJumpForce;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space, pauseKey = KeyCode.Escape, interactKey = KeyCode.F;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundLayer;
    public bool grounded;
    public float minimumFall, fallMultiplier;
    bool wasGrounded, wasFalling;
    float StartOfFall;

    bool isFalling { get { return (!grounded && rb.velocity.y < 0); } }

    [Header("Cooking")]
    public float rayRange;
    public Transform pickUpPoint;
    bool pickedUp;
    public LayerMask layerMask;
    public float minForward, maxForward, minUp, maxUp;

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Other")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public GameObject pauseMenu;
    public GameObject GUICanvas;

    public Rigidbody rb;

    private Vector3 previousPos;
    private Vector3 currentPos;

    public bool paused;

    private GameObject playerObject;
    private Camera cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        pickedUp = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.SetActive(false);

        originalSpeed = moveSpeed;
        originalAddition = sprintAddition;
        originalJumpForce = jumpForce;
        playerObject = gameObject;
        GameObject cameraObject = GameObject.Find("PlayerCam");
        cameraScript = cameraObject.GetComponent<Camera>();
        cameraScript.enabled = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        pauseMenu.SetActive(false);


        //Gets keybinds from playerprefs
        if (!PlayerPrefs.HasKey("Jump Key")) PlayerPrefs.SetString("Jump Key", "Space");
        if (!PlayerPrefs.HasKey("Pause Key")) PlayerPrefs.SetString("Pause Key", "Escape");
        if (!PlayerPrefs.HasKey("Interact Key")) PlayerPrefs.SetString("Interact Key", "F");

        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key"));
        pauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key"));
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact Key"));
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (!wasFalling && isFalling) StartOfFall = transform.position.y;
        if(!wasGrounded && grounded)
        {
            float fallDistance = StartOfFall - transform.position.y;
            if (fallDistance >= minimumFall) TakeDamage(fallMultiplier * fallDistance);
        }

        wasGrounded = grounded;
        wasFalling = isFalling;

        if (currentHealth <= 0) Die();
    }

    // Update is called once per frame
    void Update()
    {
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key"));
        pauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key"));
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact Key"));

        if (!paused)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, GroundLayer);

            MyInput();
            SpeedControl();

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

            previousPos = currentPos;

            currentPos = transform.position;
        }
    }

    public Vector3 velocityDirection
    {
        get
        {
            return (currentPos - previousPos).normalized;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        currentSpeed = moveSpeed;

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(pauseKey))
        {
            if (!paused) PauseGame();
        }

        if (Input.GetKeyDown(interactKey))
        {
            if (!pickedUp)
            {
                RaycastHit Hit = new RaycastHit();
                if (Physics.Raycast(transform.position, orientation.transform.forward, out Hit, rayRange, layerMask, QueryTriggerInteraction.Collide))
                {
                    Debug.DrawRay(transform.position, orientation.transform.forward, Color.red, 5f);
                    Debug.Log(Hit.distance);
                    if (Hit.collider.CompareTag("Throwable"))
                    {
                        pickedUp = true;
                        GameObject obj = Hit.collider.gameObject;
                        obj.transform.parent = pickUpPoint;
                        obj.transform.localPosition = Vector3.zero;
                        obj.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        obj.GetComponent<Rigidbody>().isKinematic = true;


                    }
                }
            }
            else
            {
                GameObject obj = pickUpPoint.GetChild(0).gameObject;
                obj.transform.parent = null;
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.isKinematic = false;

                Vector3 horizontalDirection = orientation.transform.forward.normalized;
                Vector3 verticalDirection = Vector3.up.normalized;
                float horizontalMagnitude = Random.Range(minForward, maxForward);
                float verticalMagnitude = Random.Range(minUp, maxUp);
                Vector3 Force = new Vector3(horizontalMagnitude * horizontalDirection.x, verticalMagnitude * verticalDirection.y, horizontalMagnitude * horizontalDirection.z);
                rb.AddForce(Force, ForceMode.Impulse);

                pickedUp = false;
            }
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10, ForceMode.Force);

        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10 * airMultiplier, ForceMode.Force);
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void ResetJump()
    {
        readyToJump = true;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        //GUICanvas.SetActive(false);
        paused = true;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        //GUICanvas.SetActive(true);
        paused = false;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Owie #" + damage);
        currentHealth -= damage;
        //Player feedback
    }

    void Die()
    {

    }
}
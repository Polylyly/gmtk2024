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
    public KeyCode jumpKey = KeyCode.Space, pauseKey = KeyCode.Escape;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundLayer;
    bool grounded;

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

        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key"));
        pauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key"));
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key"));
        pauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key"));

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
        GUICanvas.SetActive(false);
        paused = true;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GUICanvas.SetActive(true);
        paused = false;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}
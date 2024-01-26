using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3.0F;
    public float sprintSpeed = 6.0F; // The speed when sprinting
    public float rotateSpeed = 3.0F;
    public float jumpHeight = 10.0f;
    public float gravityValue = -9.81f;
    public LayerMask groundLayer; // Layer to represent the ground
    public float groundDistance = 0.2f; // Distance to check for ground
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private CharacterController controller;
    private Transform groundCheck; // Transform to represent the point from where to check for ground
    public Camera playerCamera; // Add this line
    public float crouchHeight = 0.5f; // The height of the controller when crouching
    private float originalHeight; // The original height of the controller
    public LayerMask ladderMask; // Layer to represent the ladder
    private bool isClimbing;

    // Variables for mouse look
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;
    private float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(0); // Assuming the groundCheck transform is the first child of the player
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        originalHeight = controller.height; // Save the original height of the controller
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
        move = playerBody.transform.TransformDirection(move); // Make the movement relative to the player's rotation

        // Check if the sprint key is held down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * Time.deltaTime * sprintSpeed);
        }
        else
        {
            controller.Move(move * Time.deltaTime * speed);
        }
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = crouchHeight;
        }
        else
        {
            controller.height = originalHeight;
        }

        // Ladder climbing
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, ladderMask))
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {
            moveVertical = Input.GetAxis("Vertical"); // Use the moveVertical variable that's already been declared
            Vector3 mover = new Vector3(0, moveVertical, 0);
            controller.Move(mover * Time.deltaTime * speed);
            playerVelocity.y = 0; // Prevent the player from falling due to gravity
        }
    }

}
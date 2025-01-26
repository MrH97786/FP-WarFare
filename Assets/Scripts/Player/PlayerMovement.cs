using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    // To track crouch state
    private bool crouching = false; 
    private bool lerpCrouch = false;
    // Track whether the player is sprinting
    private bool sprint = false;  

    public float speed = 5f;
    public float gravity = -10f;
    public float jumpHeight = 1.5f;
    private float crouchTimer = 0f; // Timer to manage crouch transitions

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        
        sprint = Keyboard.current.leftShiftKey.isPressed;  // True when holding Left Shift, false when released
    
        if (sprint)
            speed = 8;  // Increase speed when sprinting
        else
            speed = 5;  // Normal speed when not sprinting
        
        
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float c = crouchTimer / 1;
            c *= c;
            if (crouching)
                // When crouching, reduce height to 1
                controller.height = Mathf.Lerp(controller.height, 1, c); 
            else
                // When standing, increase height to 2
                controller.height = Mathf.Lerp(controller.height, 2, c);

            if (c > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }        
        }
    }

    //recieves input from out InputManager file and applies them to the character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2.2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.5f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    
}

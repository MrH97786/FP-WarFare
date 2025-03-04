using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool crouching = false;
    private bool lerpCrouch = false;
    private bool sprint = false;  
    private bool isDead = false;  // Track death state

    public float speed = 5f;
    public float gravity = -10f;
    public float jumpHeight = 1.5f;
    private float crouchTimer = 0f; 

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;  // If player is dead, skip movement logic

        isGrounded = controller.isGrounded;

        sprint = Keyboard.current.leftShiftKey.isPressed;  
    
        if (sprint)
            speed = 8;  
        else
            speed = 5;  

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float c = crouchTimer / 1;
            c *= c;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, c); 
            else
                controller.height = Mathf.Lerp(controller.height, 2, c);

            if (c > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }        
        }
    }

    //recieves input from InputManager and applies them to the character controller
    public void ProcessMove(Vector2 input)
    {
        if (isDead) return;  // If player is dead, skip movement

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2.2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isDead) return;  // If player is dead, skip jump

        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.5f * gravity);
        }
    }

    public void Crouch()
    {
        if (isDead) return;  // If player is dead, skip crouch

        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void SetDead(bool dead)  // Method to set player dead state
    {
        isDead = dead;
    }
}


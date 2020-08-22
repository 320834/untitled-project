using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform camera;

    public int runningSpeed;
    public int sprintingSpeed;
    private int speed;
    public float gravity = (float)(-12);
    public float jumpHeight = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    public bool isGrounded = false;

    public Vector3 velocity;

    //Reaction time for stamina
    public float regainStaminaRate = 10f;
    private float nextTimeToGainStamina = 0f;

    public float loseStaminaRate = 0.5f;
    private float nextTimeToLoseStamina = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        sprinting();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -4f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = camera.right * x + camera.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            PlayerManager.instance.player.GetComponent<PlayerStats>().decreaseStamina(10);
        }

        //First Delta time to normalize frames
        velocity.y += gravity * Time.deltaTime;

        //Second Delta time for free fall equation. Change in y = 1/2 * gravity constant * time^2
        controller.Move(velocity * Time.deltaTime);
    }

    private void sprinting()
    {
        //For sprinting
        bool can_sprint = PlayerManager.instance.player.GetComponent<PlayerStats>().validStamina();
        // UnityEngine.Debug.Log(can_sprint);

 
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) && can_sprint)
        {
            speed = sprintingSpeed;

            if (Time.time >= nextTimeToLoseStamina)
            {
                nextTimeToLoseStamina = Time.time + 1f / loseStaminaRate;
                PlayerManager.instance.player.GetComponent<PlayerStats>().decreaseStamina(2);
            }
        }
        else
        {
            speed = runningSpeed;
            if (Time.time >= nextTimeToGainStamina)
            {
                nextTimeToGainStamina = Time.time + (1f / regainStaminaRate);
                PlayerManager.instance.player.GetComponent<PlayerStats>().increaseStamina(1);
            }
        }
        
        
    }
}

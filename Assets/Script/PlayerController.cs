﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Resources;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;


public class PlayerController : MonoBehaviour
{
    // Start() varibles
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private bool isRunning = false;
    private bool isWalking = false;

    // State varibles a.k.a Finite State Machine
    private enum State { indle, walking, running, jumping, falling, hurt }
    private State state = State.indle;

    // Double tap chekcer varibles
    float buttonCooler = 0.5f;
    int buttonCountA = 0;
    int buttonCountD = 0;

    // Inspector Varibles
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private int coin = 0;
    [SerializeField] private Text coinQuantity;
    [SerializeField] private float hurtForce = 4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {



        //Debug.Log(buttonCount);

        
        if (state != State.hurt)
        {
            InputHandle();
        }
        StateSwitch();
        anim.SetInteger("state", (int)state); // Set state for enum 


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            coin += 1;
            coinQuantity.text = coin.ToString();

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Davis davis = collision.gameObject.GetComponent<Davis>(); 
            isRunning = false;
            isWalking = false;
            if (state == State.falling)
            {
                davis.JumpedOn();
                Jumping();
            }
            else
            {

                //  be hurt until indle
                // 

                state = State.hurt;
                
               
                if (collision.gameObject.transform.position.x > gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }



            }
        }

    }
    private void InputHandle()
    {

        // Jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jumping();
        }

        if (isRunning)
        {
            Movement(6f);


        }
        else if (isWalking)
        {
            Movement(3f);
        }

        // Check walking or running
        if (Input.GetKeyDown(KeyCode.A))  // Input.GetKeyDown(KeyCode.A) hDirection < 0
        {
            buttonCountA++;
            CheckMovementState();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            buttonCountD++;
            CheckMovementState();
        }

        // Double tap checker
        if (buttonCooler > 0)
        {
            buttonCooler -= 1 * Time.deltaTime;
        }
        else
        {
            buttonCountA = 0;
            buttonCountD = 0;
        }

    }
    private void CheckMovementState()
    {

        if (buttonCooler > 0 && (buttonCountA > 1 || buttonCountD > 1) && state != State.jumping)
        {
            isRunning = true;
        }
        else
        {
            isWalking = true;
            buttonCooler = 0.5f;
        }
    }
    private void Movement(float speed)
    {
        
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)  // Input.GetKeyDown(KeyCode.A) hDirection < 0
        {
            transform.localScale = new Vector2(-1, 1);
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        // Move Right
        if (hDirection > 0)
        {
            transform.localScale = new Vector2(1, 1);
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        // Double tap checker
        if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && state != State.jumping)
        {
            isRunning = false;

            rb.velocity = new Vector2(0.1f, rb.velocity.y);
        }
        
        

    }

    private void StateSwitch()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.indle;
            }
        }
        else if (state == State.hurt)
        {

            if (Mathf.Abs(rb.velocity.x) < 0.5f)
            {
                
                state = State.indle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 0.5f)
        {
            if (Mathf.Abs(rb.velocity.x) > 5f)
            {
                state = State.running;
            }
            else
            {
                state = State.walking;
            }
        }
        
        else
        {
            state = State.indle;
            
        }
    }



    private void Jumping()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        state = State.jumping;
    }
}

    

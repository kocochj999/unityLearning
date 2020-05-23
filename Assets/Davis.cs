using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Davis : MonoBehaviour
{
    [SerializeField] private float leftPoint;
    [SerializeField] private float rightPoint;

    private float speed;
    private bool facingRight = true;

    private Rigidbody2D rb;
    private Animator anim;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(anim.GetBool("isWalking"));
        if (anim.GetBool("isWalking"))
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("isWalking", false);
            }
        }

    }

    private void Move()
    {
        if (facingRight)
        {
            

            if (transform.position.x < rightPoint)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                anim.SetBool("isWalking", true);
                //Debug.Log(anim.GetBool("isWalking"));
                rb.velocity = new Vector2(3f, rb.velocity.y);
                
            }
            else
            {
                facingRight = false;
            }
        }
        else
        {
            
            
            
            if (transform.position.x > leftPoint)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                anim.SetBool("isWalking", true);
                //Debug.Log(anim.GetBool("isWalking"));
                rb.velocity = new Vector2(-3f, rb.velocity.y);
                
            }
            else
            {
                facingRight = true;
            }
        }
    }
    public void JumpedOn()
    {
        anim.SetTrigger("Dead");
    }
    private void Dead()
    {
        Destroy(this.gameObject);
    }
}

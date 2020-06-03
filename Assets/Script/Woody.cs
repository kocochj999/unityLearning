using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Woody : Enemy
{
    private bool isFacingRight = true;
    private float intiOffsetVal = 0f;
    private float intiSizeVal = 0.6f;

  
    private BoxCollider2D bc;

    

    protected override void Start()
    {
        base.Start();
        
        bc = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        
        if (anim.GetBool("isTimeToHit"))
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("isTimeToHit", false);
                bc.size = new Vector2(intiSizeVal, bc.size.y);
                bc.offset = new Vector2(intiOffsetVal, bc.offset.y);
            }
        }
        }
        
    private void Hit()
    {
        

        if (isFacingRight)
        {
            if(transform.localScale.x != 1)
            {
                transform.localScale = new Vector3(1, 1);
            }
            anim.SetBool("isTimeToHit", true);
            bc.offset = new Vector2(0.028f, bc.offset.y);
            bc.size = new Vector2(1.2f, bc.size.y);
            rb.velocity = new Vector2(2f, rb.velocity.y);
            isFacingRight = false;
        }
        else
        {
            if(transform.localScale.x != -1)
            {
                transform.localScale = new Vector3(-1, 1);
                
            }
            anim.SetBool("isTimeToHit", true);
            bc.offset = new Vector2(0.028f, bc.offset.y);
            bc.size = new Vector2(1.2f, bc.size.y);
            rb.velocity = new Vector2(-2f, rb.velocity.y);
            isFacingRight = true;
        }
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 이동")]
    [SerializeField] float moveSpeed;
    [SerializeField] bool isGround;
    [SerializeField] float groundCheckLength;
    [SerializeField] float jumpForce;

    
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0;
    
    bool isJump;
    bool doubleJump;

    Camera mainCam;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    

    
    void Update()
    {
        groundCheck();

        moving();
        Jump();
        camMoving();

        gravityCheck();

        doAnim();
    }

    /// <summary>
    /// 점프와 더블점프 
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }

        if (isJump == false && Input.GetKeyDown(KeyCode.Space) == true)
        {
            doubleJump = true;
        }
    }

    private void gravityCheck()
    {
        if (isGround == false)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
            if (verticalVelocity < -10)
            {
                verticalVelocity = -10;
            }
        }
        else if (isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }
        else if (doubleJump == true)
        {
            doubleJump = false;
            verticalVelocity = jumpForce * 2.0f;
        }
        


        else if (isGround == true)
        {
            verticalVelocity = 0;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed ;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    private void camMoving()
    {
        Vector3 playerScale = transform.localScale;
        if(playerScale.x != -1.0f && moveDir.x > 0)
        {
            playerScale.x = -1.0f;
        }
        else if(playerScale.x != 1.0f && moveDir.x < 0)
        {
            playerScale.x = 1.0f;
        }
        transform.localScale = playerScale;
    }

    private void groundCheck()
    {
        isGround = false;

        if (verticalVelocity > 0f) return;

        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center , boxCollider.bounds.size ,0f , Vector2.down , groundCheckLength , LayerMask.GetMask("Ground"));
        //박스 센터를 기준으로 박스 사이즈를 체크하고 밑에서 그라운드와 닿는지 체크

        if(hit) 
        {
            isGround = true; //닿으면 true로 변경
        }
    }


    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("isGround", isGround);
    }
}

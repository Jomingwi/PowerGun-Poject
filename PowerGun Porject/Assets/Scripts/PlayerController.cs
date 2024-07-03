using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 이동")]
    [SerializeField] float moveSpeed;
    [SerializeField] bool isGround;
    [SerializeField] float groundCheckLength;

    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0;
    
    bool isJump;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    

    
    void Update()
    {
        groundCheck();
        moving();

        gravityCheck();

        doAnim();
    }

    private void gravityCheck()
    {
        if(isGround == false)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed ;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    private void groundCheck()
    {
        isGround = false;

        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center , boxCollider.bounds.size ,0f , Vector2.down , groundCheckLength , LayerMask.GetMask("Ground"));
        //박스 센터를 기준으로 박스 사이즈를 체크하고 밑에서 그라운드와 닿는지 체크

        if(hit)
        {
            isGround = true;
        }
    }


    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("isGround", isGround);
    }
}

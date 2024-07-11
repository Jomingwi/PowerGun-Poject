using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0;


    [Header("�÷��̾� �̵�")]
    [SerializeField] float moveSpeed;
    [SerializeField] bool isGround;
    [SerializeField] float groundCheckLength;
    [SerializeField] float jumpForce;
    [SerializeField] float doublejumpForce;
    [SerializeField] float doubleJumpCoolTime = 2f;
    float doubleJumpCoolTimer = 0.0f;
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
        coolTimeCheck();
        groundCheck();
        
        moving();
        Jump();
        camMoving();
       

        gravityCheck();

        doAnim();
    }

    private void coolTimeCheck()
    {
        if (doubleJumpCoolTimer > 0f)
        {
            doubleJumpCoolTimer -= Time.deltaTime;
            if(doubleJumpCoolTimer < 0f)
            {
                doubleJumpCoolTimer = 0f;
            }
        }
    }



    /// <summary>
    /// ������ �������� 
    /// ���������� �ٴڿ� �����ʰ� ��Ÿ���� ���ƾ��ϰ� �����̽��ٸ� ������ ����
    /// ������ ����ϴ°��� �����ϱ����� �ѹ� ������ �ٴڿ� ���� ������ return�ϵ��� ����
    /// </summary>
    private void Jump()
    {
        if (isGround == false)
        {
            if (doubleJumpCoolTimer ==0 && Input.GetKeyDown(KeyCode.Space) == true)
            {
                doubleJump = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) == true )
        {
            isJump = true;
        }
        
    }

    private void gravityCheck()
    {
        
        
        if (doubleJump == true)
        {
            doubleJump = false;
            verticalVelocity = doublejumpForce;
            doubleJumpCoolTimer = doubleJumpCoolTime;
        }

        else if (isGround == false)
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
        else if (isGround == true)
        {
            verticalVelocity = 0;
        }
        rigid.velocity = new Vector2(rigid.velocity.x , verticalVelocity);
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
        //�ڽ� ���͸� �������� �ڽ� ����� üũ�ϰ� �ؿ��� �׶���� ����� üũ

        if(hit) 
        {
            isGround = true; //������ true�� ����
        }
    }


    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("isGround", isGround);
    }
}

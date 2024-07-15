using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxColl;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0;

    GameObject fabExplosion;
    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    


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
    float spikeHitCoolTime = 1;
    float spikeHitCoolTimer = 0.0f;
 

    [Header("�÷��̾� ����")]
    [SerializeField] float maxHp = 100;
    [SerializeField] float curHp;
    bool isSpike;
    bool spikeHit;



    [Header("dash")]
    [SerializeField] Image dashImageFill;
    [SerializeField] TMP_Text textDashCoolTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashCoolTime = 5f;
    [SerializeField] float dashTime = 0.5f;
    float dashTimer = 0f;
    float dashCoolTimer= 0f;
    bool isDash;
   

    [Header("�����̵�")]
    [SerializeField] Image slideImageFill;
    [SerializeField] TMP_Text textSlideCoolTime;
    [SerializeField] float slideSpeed = 10f;
    [SerializeField] float slideCoolTime =2;
    [SerializeField] float slideTime = 0.5f;
    float slideTimer = 0f;
    float slideCoolTimer= 0f;
    bool isSlide;

    Camera mainCam;


    public void TriggerEnter(HitBox.ehitType type, Collider2D other)
    {
        if(type == HitBox.ehitType.bodyCheck)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
            {
                isSpike = true;
                Hit();
            }
            if (other.tag == Tool.GetTag(GameTags.Enemy))
            {
                Hit();
            }
        }
       
    }

    private void Hit()
    {
        curHp--;
        if (curHp <= 0)
        {
            Destroy(gameObject);
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            goSc.ImageSize(spriteRenderer.sprite.rect.width);
        }
    }




    public void TriggerExit(HitBox.ehitType type , Collider2D other) 
    {
        if (type == HitBox.ehitType.spikeCheck)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
            {
                isSpike = false;
            }
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        initUI();

        curHp = maxHp;
    }

    void Start()
    {
        mainCam = Camera.main;
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
        gameManager.Player = this;
    }

    
    void Update()
    {
        coolTimeCheck();
        groundCheck();
        spike();

        moving();
        jump();
        dash();
        slide();

        camMoving();
      
        gravityCheck();

        doAnim();
    }

    
   


    private void spike()
    {
        if (isSpike == true)
        {
            spikeHit = true;
            spikeHitCoolTimer = spikeHitCoolTime;
            rigid.velocity = new Vector2(transform.localScale.x > 0 ? 5 : -5 , jumpForce);
        }
        if(spikeHitCoolTimer == 0f)
        {
            spikeHit = false;
        }
    }


    private void slide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)  == true && slideCoolTimer == 0f  && slideTimer == 0f )
        {
            if(isGround == false) { return; }
            isSlide = true;
            slideCoolTimer = slideCoolTime;
            slideTimer = slideTime;
            verticalVelocity = 0;
            rigid.velocity = new Vector2(transform.localScale.x > 0 ? -slideSpeed : slideSpeed, verticalVelocity);
        }
        if(slideTimer == 0f)
        {
            isSlide = false;
        }
    }

    private void dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) == true && dashCoolTimer == 0f && dashTimer == 0f )
        {
            isDash = true;
            dashCoolTimer = dashCoolTime;
            dashTimer = dashTime;
            verticalVelocity = 0f;
            rigid.velocity = new Vector2(transform.localScale.x > 0 ? -dashSpeed : dashSpeed, verticalVelocity);
        }
        if (dashTimer == 0f)
        {
            isDash = false;
        }
    }


    private void coolTimeCheck()
    {
        if(spikeHitCoolTimer > 0f )
        {
            spikeHitCoolTimer -= Time.deltaTime;
            if(spikeHitCoolTimer < 0f )
            {
                spikeHitCoolTimer = 0f;
            }
        }

        if (doubleJumpCoolTimer > 0f)
        {
            doubleJumpCoolTimer -= Time.deltaTime;
            if(doubleJumpCoolTimer < 0f)
            {
                doubleJumpCoolTimer = 0f;
            }
        }

        if(dashCoolTimer > 0f)
        {
            
            dashCoolTimer -= Time .deltaTime;
            if(dashCoolTimer < 0f)
            {
                dashCoolTimer = 0f;
                
            }
            dashImageFill.fillAmount = 1 - dashCoolTimer / dashCoolTime;
            textDashCoolTime.text = dashCoolTimer.ToString("F1");
            textDashCoolTime.enabled = true;
        }
        if (dashCoolTimer == 0f)
        {
            textDashCoolTime.enabled = false;
        }

        if (dashTimer > 0f)
        {
            dashTimer -= Time .deltaTime;
            if(dashTimer < 0f)
            {
                dashTimer = 0f;
            }
        }

        if(slideTimer > 0f)
        { 
            slideTimer -= Time .deltaTime; 
            if(slideTimer < 0f)
            {
                slideTimer = 0f;
            }
        }

        if(slideCoolTimer > 0f )
        {
           
            slideCoolTimer -= Time .deltaTime;
            if (slideCoolTimer < 0f)
            {
                slideCoolTimer = 0f;
            }

            slideImageFill.fillAmount = 1 - slideCoolTimer / slideCoolTime;
            textSlideCoolTime.text = slideCoolTimer.ToString("F1");
            textSlideCoolTime.enabled = true;
        }
        if(slideCoolTimer == 0f)
        {
            textSlideCoolTime.enabled = false;
        }

    }



    /// <summary>
    /// ������ �������� 
    /// ���������� �ٴڿ� �����ʰ� ��Ÿ���� ���ƾ��ϰ� �����̽��ٸ� ������ ����
    /// ������ ����ϴ°��� �����ϱ����� �ѹ� ������ �ٴڿ� ���� ������ return�ϵ��� ����
    /// </summary>
    private void jump()
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

    /// <summary>
    /// �÷��̾ ���������� �������� �ӵ��� �߷� üũ
    /// </summary>
    private void gravityCheck()
    {
        if (dashTimer > 0f || slideTimer > 0f )
        {
            return;
        }
        

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

    /// <summary>
    /// �÷��̾� �����Ӱ� �ӵ�
    /// </summary>
    private void moving()
    {
        if (dashTimer > 0 || slideTimer > 0 || spikeHitCoolTimer > 0)
        { 
            return; 
        }

        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed ;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    /// <summary>
    /// �÷��̾ �������κ��� �÷��̾��� �ڽ��� ī�޶�� �ڽĵ��� �������� ����
    /// ���������� �����̸� ���������� ������ ����
    /// </summary>
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

    /// <summary>
    /// �÷��̾ �ٴڰ� ����� üũ
    /// </summary>
    private void groundCheck()
    {

        isGround = false;

        if (verticalVelocity > 0f) return;

        RaycastHit2D hit = 
            Physics2D.BoxCast(boxColl.bounds.center , boxColl.bounds.size ,0f , Vector2.down , groundCheckLength , LayerMask.GetMask("Ground"));
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
        anim.SetBool("isDash", isDash);
        anim.SetBool("isSlide", isSlide);
        anim.SetBool("isSpike", spikeHit);
    }

    private void initUI()
    {
        dashImageFill.fillAmount = 1;
        textDashCoolTime.text = "";
        textDashCoolTime.enabled = false;

        slideImageFill.fillAmount = 1;
        textSlideCoolTime.text = "";
        textSlideCoolTime.enabled = false;
    }

}

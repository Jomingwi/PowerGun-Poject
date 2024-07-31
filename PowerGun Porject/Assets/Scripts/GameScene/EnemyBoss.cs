using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : MonoBehaviour
{
  [Header("보스 설정")]
  [SerializeField] float moveSpeed;
  [SerializeField] public float bossMaxHp;
  [SerializeField] public float bossCurHp;
  [SerializeField] float groundCheckLength;
  Transform trsBossPos;
  float timer = 0;
  float patternTime = 0;


  [SerializeField] bool isGround = false;
  bool isbossMoving = false;
  Vector2 moveDir = new Vector2(1, 0);

  [Header("돌진")]
  [SerializeField] int dashSpeed;
  bool isbossDash;

  [Header("무기 던지기")]
  [SerializeField] Vector2 throwForce;
  [SerializeField] GameObject objThrowWeapon;
  [SerializeField] Transform trsWeapon;
  [SerializeField] Transform trsHand;


  [Header("점프")]
  float verticalVelocity = 0;
  [SerializeField] int jumpForce;
  bool isJump = false;


  [Header("플레이어 기절")]
  [SerializeField] float playerStunTime;
  [SerializeField] float playerStunTimer;
  bool isStun = false;

  [Header("게임 완료")]
  [SerializeField] Image gameClear;
  [SerializeField] TMP_Text textClear;




  GameObject fabExplosion;
  GameManager gameManager;
  Animator anim;
  Rigidbody2D rigid;
  SpriteRenderer spriteRenderer;
  BoxCollider2D boxcoll;

  Difficulty difficulty;
  int curPattern = 0;
  bool patternActive = false;

  

  public void difficultyHp(Difficulty difficulty)
  {
    if (difficulty == Difficulty.Easy) { bossMaxHp *= 0.7f; }
    else if (difficulty == Difficulty.Hard) { bossMaxHp *= 1.5f; }
    bossCurHp = bossMaxHp;
  }

  


  private void Awake()
  {
    rigid = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    boxcoll = GetComponent<BoxCollider2D>();
    spriteRenderer = transform.GetComponent<SpriteRenderer>();

  }



  void Start()
  {
    gameManager = GameManager.Instance;
    trsBossPos = gameManager.TrsBossPos;
    fabExplosion = gameManager.FabExplosion;

  }




  void Update()
  {
    if (gameObject == null) return;
    bossGravityCheck();
    bossGroundCheck();


    bossMoving();
    if (patternActive == false) { nextPattern(gameManager.curDifficulty); }


    executePattern();


    doAnim();
  }

  public void nextPattern(Difficulty difficulty)
  {
    if (difficulty == Difficulty.Easy)
    {
      curPattern = Random.Range(0, 2);
    }
    else if (difficulty == Difficulty.Normal)
    {
      curPattern = Random.Range(0, 3);
    }
    else if (difficulty == Difficulty.Hard)
    {
      curPattern = Random.Range(0, 4);
    }
    patternActive = true;
  }

  private void executePattern()
  {

    timer += Time.deltaTime;
    if (timer < 5)
    {
      return;
    }

    if (curPattern == 0) { bossDash(); }
    else if (curPattern == 1) { createWeapon(); }
    else if (curPattern == 2) { jump(); }
    else if (curPattern == 3) { stun(); }

    Debug.Log(curPattern);
    timer = 0;
    patternActive = false;
    isbossDash = false;
    isJump = false;

   if(patternTime > 1)
    {
      patternTime = 0;
    }
  }

  






  /// <summary>
  ///보스가 돌진
  /// </summary>
  private void bossDash()
  {
    patternTime += Time.deltaTime;

    isbossDash = true;
    Vector3 playerPos = gameManager.Player.transform.position;
    Vector3 distance = playerPos - transform.position;
    distance.x = distance.x < 0 ? -1 : 1;

    rigid.velocity = new Vector2(distance.x * dashSpeed, rigid.velocity.y);

    Vector3 scale = transform.localScale;
    scale.x = playerPos.x < transform.position.x ? -1 : 1;
    transform.localScale = scale;
  }

  private void createWeapon()
  {
    patternTime += Time.deltaTime;
    Vector3 playerPos = gameManager.Player.transform.position;
    Vector3 distance = playerPos - transform.position;

    float angle = Quaternion.FromToRotation(
        transform.localScale.x < 0 ? Vector2.right : Vector2.left, distance).eulerAngles.z;
    trsHand.rotation = Quaternion.Euler(0, 0, angle);

    GameObject go = Instantiate(objThrowWeapon, trsWeapon.position, trsWeapon.rotation, gameManager.trsSpawnPos);
    EnemyThowWeapon goSc = go.GetComponent<EnemyThowWeapon>();
    bool isRight = transform.localScale.x < 0 ? true : false;
    Vector2 fixedThrowforce = throwForce;
    if (isRight == false)
    {
      fixedThrowforce = -throwForce;
    }
    goSc.SetForce(trsWeapon.rotation * fixedThrowforce, isRight);

  }




  private void jump()
  {
    patternTime += Time.deltaTime;
    isJump = true;

    Vector3 playerPos = gameManager.Player.transform.position;
    Vector3 distance = playerPos - transform.position;

    Vector2 horizontalForce = rigid.velocity;
    horizontalForce.x = distance.x;

    verticalVelocity = jumpForce;

    rigid.velocity = new Vector2(distance.x, verticalVelocity);
  }

  public void stun()
  {
    patternTime += Time.deltaTime;
    if (patternTime > 0)
    {
      gameManager.Player.playerStun(true);
    }
    
     

  }




  private void bossGroundCheck()
  {
    isGround = false;
    if (verticalVelocity > 0) { return; }

    RaycastHit2D hit =
        Physics2D.BoxCast(boxcoll.bounds.center, boxcoll.bounds.size, 0, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

    if (hit)
    {
      isGround = true;
    }
  }

  private void bossGravityCheck()
  {
    if (isGround == false)
    {
      verticalVelocity += Physics.gravity.y * Time.deltaTime;
      if (verticalVelocity < -10)
      {
        verticalVelocity = -10;
      }
    }
    else if (isGround == true)
    {
      verticalVelocity = 0;
    }
    rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    
   
  }





  private void bossMoving()
  {
    
    if (isGround == false || patternTime > 0) { return; }
    isbossMoving = true;

    Vector3 playerPos = gameManager.Player.transform.position;
    Vector3 distance = playerPos - transform.position;
    distance.x = distance.x < 0 ? -1 : 1;

    rigid.velocity = new Vector2(distance.x * moveSpeed, rigid.velocity.y);

    Vector3 scale = transform.localScale;
    scale.x = playerPos.x < transform.position.x ? -1 : 1;
    transform.localScale = scale;
  }


  public void bossHit(float damage)
  {
    bossCurHp -= damage;

    if (bossCurHp < 0)
    {
      bossCurHp = 0;
    }

    gameManager.bossModifySlider(bossCurHp);

    if (bossCurHp <= 0)
    {
      Destroy(gameObject);

      GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
      Explosion goSc = go.GetComponent<Explosion>();
      goSc.ImageSize(spriteRenderer.sprite.rect.width);

      gameManager.GameOver();
    }

  }
   


  private void doAnim()
  {
    anim.SetBool("isbossMoving", isbossMoving);
    anim.SetBool("isbossDash", isbossDash);
  }

}

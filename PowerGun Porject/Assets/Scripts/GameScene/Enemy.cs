using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum eEnemyType
    {
        EnemyFly,
        EnemyWalk,
        EnemyDragon,
        EnemyBoss,
    }


    [Header("적 설정")]
    [SerializeField] eEnemyType enemyType;

    [SerializeField] float moveSpeed;
    [SerializeField] public float enemyCurHp;
    [SerializeField] public float enemyMaxHP;
    float beforeHP;
    [SerializeField] float moveTimer = 0;
    float moveTime = 3;

    
    GameObject fabExplosion;
    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    EnemyHp enemyHP;

    Rigidbody2D rigid;
    BoxCollider2D boxcoll;
    Vector3 moveDir = new Vector2(1, 0);
    Camera mainCam;


    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        boxcoll = GetComponentInChildren<BoxCollider2D>();
        enemyCurHp = enemyMaxHP;
        moveTimer = moveTime;
    }



    private void Start()
    {
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
        mainCam = Camera.main;
        setHpBar();
    }


    private void Update()
    {
        enemyMoving();
    }

    public void setHpBar()
    {
        if(gameManager == null) { return; }

        GameObject go = Instantiate(gameManager.FabEnemyHp, gameManager.trsCanvas.transform.position, Quaternion.identity, gameManager.trsHpBarPos); ;
        EnemyHp goSc = go.GetComponent<EnemyHp>();
        goSc.SetEnemy(this);
    }
    

    public void Hit(float damage)
    {
        if(enemyCurHp < 0)
        {
            enemyCurHp = 0;
        }
       
        enemyCurHp -= damage;
        gameManager.SetEnemyHp(enemyMaxHP ,enemyCurHp);

        if (enemyCurHp == 0f)
        {
            Destroy(gameObject);
            

            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();
            goSc.ImageSize(spriteRenderer.sprite.rect.width);

            gameManager.enemyKillCount();
        }

    }


    /// <summary>
    /// 에너미가 움직이는 코드
    /// </summary>
    private void enemyMoving()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer < 0f)
        {
            movingCheck();
            moveTimer = moveTime;
        }
        if (boxcoll.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            movingCheck();
        }
        rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
    }

    /// <summary>
    /// 방향을 반대로 돌림
    /// </summary>
    private void movingCheck()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        moveDir.x *= -1;
    }

    private void playerCheckPos()
    {
        Vector3 pos;
        if (gameManager.GetPlayerPos(out pos) == true)
        {
            Vector2 distance = pos - transform.position;
            mainCam.ViewportToWorldPoint(distance);
            transform.position = pos;

        }
        else
        {
            enemyMoving();
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
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


    [Header("Àû ¼³Á¤")]
    [SerializeField] eEnemyType enemyType;
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyHP;
    [SerializeField] float enemyMaxHP;
    [SerializeField] Image imgEnemyHP;
    [SerializeField] float moveTimer = 0;
    float moveTime = 3;
    bool isMoving;
    bool isSway;
  

    bool isDie;
    GameObject fabExplosion;
    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    MapBound map;

    Rigidbody2D rigid;
    BoxCollider2D boxcoll;
    Vector3 moveDir = new Vector2(1,0);
    Camera mainCam;


 


    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        boxcoll = GetComponentInChildren<BoxCollider2D>();
        enemyHP = enemyMaxHP;
        moveTimer = moveTime;
        initUI();
    }

    private void initUI()
    {
        imgEnemyHP.fillAmount = 1;
        
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
        mainCam = Camera.main;
    }


    private void Update()
    {
        enemyMoving();
    }

    public void SetEnemyHp(float maxHp, float curHp)
    {
        curHp = enemyHP;
        imgEnemyHP.fillAmount = curHp;
    }


    public void Hit(float damage)
    {
        if (isDie == true)
        {
            return;
        }
        enemyHP -= damage;
        gameManager.SetEnemyHp(enemyHP, enemyMaxHP);

        if (enemyHP <= 0f)
        {
            isDie = true;
            Destroy(gameObject);

            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();
            goSc.ImageSize(spriteRenderer.sprite.rect.width);

            gameManager.enemyKillCount();
        }
    }

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
       
    }

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
        if(gameManager.GetPlayerPos(out pos) == true)
        {
            Vector2 distance = pos - transform.position;
            mainCam.ViewportToWorldPoint(distance);
            transform.position = pos;
            
        }
        else
        {
            enemyMoving();
        }
        rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
    }
    
}

   



   









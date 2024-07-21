using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    [Header("�� ����")]
<<<<<<< HEAD
    [SerializeField] eEnemyType enemyType;
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyCurHp = 0;
    [SerializeField] float enemyMaxHP;
    [SerializeField] public Image imgEnemyHP;
    [SerializeField] float movingTime = 3;
    float movingTimer = 0;

=======
    [SerializeField]  eEnemyType enemyType;
    [SerializeField]  float moveSpeed;
    [SerializeField] float enemyHP;
    float enemyMaxHP;
    [SerializeField]  Image imgEnemyHP;
>>>>>>> parent of e705cee (동기화)

    bool isDie;
    GameObject fabExplosion;
    GameManager gameManager;
    SpriteRenderer spriteRenderer;

    Rigidbody2D rigid;
<<<<<<< HEAD
    BoxCollider2D boxcoll;
    Vector2 moveDir = new Vector2(1, 0);
    Camera mainCam;



=======
    float verticalVelocity;
    Vector2 moveDir;
>>>>>>> parent of e705cee (동기화)


    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        boxcoll = GetComponentInChildren<BoxCollider2D>();
        enemyCurHp = enemyMaxHP;
        movingTimer = movingTime;
=======
        enemyHP = enemyMaxHP;
        initUI();
    }

    private void initUI()
    {
        imgEnemyHP.fillAmount = 1;
>>>>>>> parent of e705cee (동기화)
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
    }

<<<<<<< HEAD

    private void Update()
    {
        enemyMoving();
    }

    public void SetEnemyHp(float maxHp, float curHp)
=======
    public void SetEnemyHp(float maxHp , float curHp)
>>>>>>> parent of e705cee (동기화)
    {
        curHp = enemyCurHp;
        imgEnemyHP.fillAmount = curHp;
    }

    public void Hit(float damage)
    {
        if(isDie == true)
        {
            return;
        }
<<<<<<< HEAD
        enemyCurHp -= damage;
        gameManager.SetEnemyHp(enemyMaxHP, enemyCurHp);
=======
        enemyHP -= damage;
        
>>>>>>> parent of e705cee (동기화)

        if (enemyCurHp <= 0f)
        {
            isDie = true;
            Destroy(gameObject);
            
            GameObject go = Instantiate(fabExplosion, transform.position , Quaternion.identity , transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();
            goSc.ImageSize(spriteRenderer.sprite.rect.width);

            gameManager.enemyKillCount();
        }
    }

<<<<<<< HEAD
    /// <summary>
    /// 
    /// </summary>
    private void enemyMoving()
    {
        movingTimer -= Time.deltaTime;
        if (movingTimer < 0f)
        {
            movingCheck();
            movingTimer = movingTime;
        }
        if (boxcoll.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            movingCheck();
        }
        rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
    }

    private void movingCheck()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        moveDir.x *= -1;
    }
=======
    

>>>>>>> parent of e705cee (동기화)

}

       












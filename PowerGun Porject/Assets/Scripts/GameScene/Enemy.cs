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


    [Header("Àû ¼³Á¤")]
    [SerializeField] eEnemyType enemyType;
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyHP;
    [SerializeField] float enemyMaxHP;
    [SerializeField] Image imgEnemyHP;
    bool isMoving;
    bool isSway;
  

    bool isDie;
    GameObject fabExplosion;
    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    MapBound map;

    Rigidbody2D rigid;
    BoxCollider2D boxcoll;
    Vector3 moveDir = new Vector2(0.5f,0);
    Camera mainCam;


 


    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        boxcoll = GetComponentInChildren<BoxCollider2D>();
        enemyHP = enemyMaxHP;
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
        if (imgEnemyHP.fillAmount <= 0)
        {
            Destroy(gameObject);
        }
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
        if (isMoving == true)
        {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            
            if (boxcoll.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
            rigid.velocity = new Vector2(moveDir.x , rigid.velocity.y);
        }
        else
        {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        }
    }

    private void movingCheck()
    {
        if(map == null)
        {
            map = gameManager.Map;
        }
    }



   







}

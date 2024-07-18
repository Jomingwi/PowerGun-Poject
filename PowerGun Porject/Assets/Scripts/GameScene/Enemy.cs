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
    [SerializeField]  eEnemyType enemyType;
    [SerializeField]  float moveSpeed;
    [SerializeField] float enemyHP;
    float enemyMaxHP;
    [SerializeField]  Image imgEnemyHP;

    bool isDie;
    GameObject fabExplosion;
    GameManager gameManager;
    SpriteRenderer spriteRenderer;

    Rigidbody2D rigid;
    float verticalVelocity;
    Vector2 moveDir;


    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
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
    }

    public void SetEnemyHp(float maxHp , float curHp)
    {
        curHp = enemyHP;
        imgEnemyHP.fillAmount = curHp;
    }

    public void Hit(float damage)
    {
        if(isDie == true)
        {
            return;
        }
        enemyHP -= damage;
        

        if (enemyHP <= 0f)
        {
            isDie = true;
            Destroy(gameObject);
            
            GameObject go = Instantiate(fabExplosion, transform.position , Quaternion.identity , transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();
            goSc.ImageSize(spriteRenderer.sprite.rect.width);

            gameManager.enemyKillCount();
        }
    }

    


}

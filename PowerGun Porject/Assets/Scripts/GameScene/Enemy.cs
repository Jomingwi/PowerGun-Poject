using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum eEnemyType
    {
        EnemyFly,
        EnemyWalk,
        EnemyDragon,
        EnemyBoss,
    }

    [SerializeField] protected eEnemyType enemyType;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float enemyHP;
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

    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
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

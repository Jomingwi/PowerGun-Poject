using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    Enemy enemy;
    [SerializeField] Image fabEnemyImage;
    
    private void Awake()
    {
        initHp();
    }
   
    private void Update()
    {
        ChaseEnemy();
        enemyDie();
    }

    public void SetEnemy(Enemy _enemy)
    {
        enemy = _enemy;
    }


    private void initHp()
    {
        fabEnemyImage.fillAmount = 1;
    }

    public void ChaseEnemy()
    {
        if(enemy != null)
        {
            Vector3 pos;
            pos = enemy.transform.position;
            pos.y += 0.5f;
            transform.position = pos;
        }

    }


    public void SetEnemyHp(float _maxHp, float _curHp)
    {
        fabEnemyImage.fillAmount = _curHp / _maxHp;
    }

    private void enemyDie()
    {
        if (fabEnemyImage.fillAmount == 0)
        {
            Destroy(gameObject);
        }
    }


}

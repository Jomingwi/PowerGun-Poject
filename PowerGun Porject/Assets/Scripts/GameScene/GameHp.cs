using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class GameHp : MonoBehaviour
{
    [SerializeField] Image imgEnemyHp;
    [SerializeField] Image imgEnemyEffect;

    [SerializeField] float effectTime;
    GameManager gameManager;
    Enemy enemy;


    private void Awake()
    {
        initHp();
    }


    void Start()
    {
        gameManager = GameManager.Instance;
        chaseEnemy();
    }

    void initHp()
    {
        imgEnemyHp.fillAmount = 1;
    }

   
    void Update()
    {
        checkEnemyDestroy();
    }

    /// <summary>
    /// 에너미 포지션 값을 받아와서 그게 참이라면 에너미포지션에서 -0.7한 값으로 생성
    /// </summary>
    public void chaseEnemy()
    {
        if(gameManager.EnemyPos(out Vector2 pos) == true)
        {
            pos.y -= 0.7f;
            transform.position = pos;
        }
    }


    public void SetHp(float _maxhp , float _curHp)
    {
         imgEnemyHp.fillAmount = _curHp / _maxhp;
    }

    private void checkEnemyDestroy()
    {
        if(imgEnemyHp.fillAmount == 0f)
        {
            Destroy(gameObject);
        }
    }
}

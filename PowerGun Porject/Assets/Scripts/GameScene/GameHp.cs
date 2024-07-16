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
    /// ���ʹ� ������ ���� �޾ƿͼ� �װ� ���̶�� ���ʹ������ǿ��� -0.7�� ������ ����
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

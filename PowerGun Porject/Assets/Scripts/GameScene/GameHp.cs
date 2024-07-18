using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class GameHp : MonoBehaviour
{
    [Header("적 체력")]
    [SerializeField] Image imgEnemyHp;
    [SerializeField] Image imgEnemyEffect;

    [SerializeField] float effectTime;
    GameManager gameManager;
    Enemy enemy;

    [Header("플레이어 체력")]
    [SerializeField] Image imgPlayerHP;
    [SerializeField] TMP_Text textPlayerHP;



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
        imgPlayerHP.fillAmount = 1;
    }

   
    void Update()
    {
        checkPlayerDestroy();
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


    public void SetPlayerHp(float _maxhp , float _curHp)
    {
        textPlayerHP.text = (_maxhp - _curHp).ToString( "F3") ;
        imgPlayerHP.fillAmount = _curHp / _maxhp;
    }

    public void SetEnemyHp(float _maxhp, float _curHp)
    {
        imgEnemyHp.fillAmount = _curHp / _maxhp;

    }


    private void checkPlayerDestroy()
    {
        if(imgPlayerHP.fillAmount == 0f)
        {
            Destroy(gameObject);
        }
    }
}

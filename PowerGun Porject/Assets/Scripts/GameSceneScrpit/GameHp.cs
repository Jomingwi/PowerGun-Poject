using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHp : MonoBehaviour
{
    [SerializeField] Image imgEnemyHp;
    [SerializeField] Image imgEnemyEffect;

    [SerializeField] float effectTime;
    GameManager gameManager;


    private void Awake()
    {
        initHp();
    }


    void Start()
    {
        gameManager = GameManager.Instance;
        
    }

    void initHp()
    {
        imgEnemyHp.fillAmount = 1;
    }

   
    void Update()
    {
        checkEnemyDestroy();
    }

    private void chaseEnemy()
    {
        
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

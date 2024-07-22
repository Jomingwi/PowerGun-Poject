using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] Image img;


    private void Awake()
    {
        initHp();
    }
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        
        enemyDie();
    }


    private void initHp()
    {
        img.fillAmount = 1;
    }


    public void SetEnemyHp(float maxHp, float curHp)
    {
        img.fillAmount = curHp / maxHp;
    }

    private void enemyDie()
    {

        if (img.fillAmount <= 0)
        {
            Destroy(gameObject);
        }
    }


}

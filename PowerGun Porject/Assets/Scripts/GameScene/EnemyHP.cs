using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]Image img;
    

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
        EnemyHpBar();
        enemyDie();
    }


    private void initHp()
    {
        img.fillAmount = 1;
    }


    public void EnemyHpBar()
    {
        
        if (gameManager.Enemy != null)
        {
            Instantiate(img, transform.position, Quaternion.Euler(0, 0, 0), transform.parent);
            img.enabled = true;
        }
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

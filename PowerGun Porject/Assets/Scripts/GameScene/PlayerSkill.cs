using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill: MonoBehaviour
{


    [SerializeField] Transform dynamicObject;
    [SerializeField] Transform trsAttack;

    bool skillshot;

    [Header("Çìµå¼¦")]
    [SerializeField] GameObject skillHeadShot;
    [SerializeField] float skillHeadShotCoolTime = 5;
    float skillHeadShotCoolTimer;
    bool isHeadShot;

    [Header("¿¬»ç")]
    [SerializeField] GameObject skillTen;
     [SerializeField] float skillTenCoolTime = 5;
    float skillTenCoolTimer;
    bool isTen;

    [Header("¼¦°Ç")]
    [SerializeField] GameObject skillShotGun;
    [SerializeField] float skillShotGunCoolTime = 5;
    float skillShotGunCoolTimer;
    bool isShotGun;


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (skillshot == false && collision.tag == "Enemy")
        {
            if(isHeadShot == true )
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Hit(15);
            }
            if(isTen == true)
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Hit(3);
            }
            if(isShotGun ==true)
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Hit(30);
            }
           
        }
        if (skillshot == true && collision.tag == "Player")
        {
            Destroy(gameObject);
            PlayerController player = collision.GetComponent<PlayerController>();
            player.Hit();
        }
    }




    void Start()
    {
        
    }

  
    void Update()
    {
        skillCoolTimeCheck();
        skillKeySetting(); 
    }


    private void skillKeySetting()
    {
        if (Input.GetKey(KeyCode.X))
        {
            isHeadShot = true;
            playerSkill();
        }
        if(Input.GetKey(KeyCode.C)) 
        {
            isTen = true;
            playerSkill();
        }
        if(Input.GetKey(KeyCode.V)) 
        {
            isShotGun = true;
            playerSkill();
        }

    }



    private void playerSkill()
    {
        if(isHeadShot == true && skillHeadShotCoolTimer == 0)
        {
            skillHeadShotCoolTimer = skillHeadShotCoolTime;
        }

        if (isTen == true && skillTenCoolTimer == 0)
        {
            skillTenCoolTimer = skillTenCoolTime;
        }

        if(isShotGun == true && skillShotGunCoolTimer == 0)
        {
            skillShotGunCoolTimer = skillShotGunCoolTime;
        }
    }


    private void skillCoolTimeCheck()
    {
        if(skillHeadShotCoolTimer  > 0)
        {
            skillHeadShotCoolTimer -= Time.deltaTime;
            if(skillHeadShotCoolTimer < 0)
            {
                skillHeadShotCoolTimer = 0;
            }
        }

        if(skillTenCoolTimer> 0)
        {
            skillTenCoolTimer -= Time.deltaTime;
            if (skillTenCoolTimer < 0)
            {
                skillTenCoolTimer = 0;
            }
        }

        if(skillShotGunCoolTimer> 0)
        {
            skillShotGunCoolTimer -= Time.deltaTime;
            if (skillShotGunCoolTimer < 0)
            {
                skillShotGunCoolTimer = 0;
            }
        }
            
    }

    private void skillCoolTimeUI()
    {

    }


    public void skillShot()
    {
        skillshot = false;
    }

}

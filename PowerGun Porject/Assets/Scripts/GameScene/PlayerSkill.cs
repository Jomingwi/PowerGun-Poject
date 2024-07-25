using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    Quaternion angle = Quaternion.Euler(0, 0, 180);

    [SerializeField] Transform dynamicObject;
    [SerializeField] Transform trsAttack;

    bool skillshot = false;

    [Header("헤드샷")]
    [SerializeField] GameObject skillHeadShot;
    [SerializeField] float skillHeadShotCoolTime = 5;
    [SerializeField] Image headShotImgFill;
    [SerializeField] TMP_Text textHeadShotCoolTime;
    float skillHeadShotCoolTimer;
    bool isHeadShot;

    [Header("연사")]
    [SerializeField] GameObject skillTen;
    [SerializeField] float skillTenCoolTime = 5;
    [SerializeField] Image tenImgFill;
    [SerializeField] TMP_Text textTenCoolTime;
    float skillTenCoolTimer;
    bool isTen;

    bool isSetTenSkill = false;
    float curLookAtPos;
    int remainTenSkill;
    float timerTenSkill;//타이머
    float timeTenSkill = 0.1f;//발사간격


    [Header("샷건")]
    [SerializeField] GameObject skillShotGun;
    [SerializeField] float skillShotGunCoolTime = 5;
    [SerializeField] Image shotGunImgFill;
    [SerializeField] TMP_Text textShotGunCoolTime;
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
            if (isHeadShot == true)
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
                EnemyBoss boss = collision.GetComponent<EnemyBoss>();
                enemy.Hit(15);
                boss.bossHit(15);
            }
            if (isTen == true)
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
				        EnemyBoss boss = collision.GetComponent<EnemyBoss>();
				        enemy.Hit(3);
                boss.bossHit(3);
            }
            if (isShotGun == true)
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
				        EnemyBoss boss = collision.GetComponent<EnemyBoss>();
				        enemy.Hit(3);
                boss.bossHit(3);
            }

        }
    }
    private void Awake()
    {
        initUI();
    }


    void Update()
    {
        skillCoolTimeCheck();
        skillKeySetting();

        //doSkill();
    }

    private void doSkill()
    {
            Ten();
    }

    public void skillKeySetting()
    {

        if (Input.GetKeyDown(KeyCode.X) && skillHeadShotCoolTimer == 0)
        {
            isHeadShot = true;
            playerSkill();
            headShot();
        }
        if (Input.GetKeyDown(KeyCode.C) && skillTenCoolTimer == 0)
        {
            isTen = true;
            skillTenCoolTimer = skillTenCoolTime;
            remainTenSkill = 10;
            StartCoroutine(corSkillTen());

            //playerSkill();
            //Ten();
        }
        if (Input.GetKeyDown(KeyCode.V) && skillShotGunCoolTimer == 0)
        {
            isShotGun = true;
            playerSkill();
            ShotGun();
        }

    }



    private void playerSkill()
    {
        if (isHeadShot == true)
        {
            isHeadShot = false;
            skillHeadShotCoolTimer = skillHeadShotCoolTime;
        }

        //if (isTen == true)
        //{
        //    //isTen = false;
        //    skillTenCoolTimer = skillTenCoolTime;
        //}

        if (isShotGun == true)
        {
            isShotGun = false;
            skillShotGunCoolTimer = skillShotGunCoolTime;
        }
    }


    private void skillCoolTimeCheck()
    {
        if (skillHeadShotCoolTimer > 0)
        {
            skillHeadShotCoolTimer -= Time.deltaTime;
            if (skillHeadShotCoolTimer < 0)
            {
                skillHeadShotCoolTimer = 0;
            }
            headShotImgFill.fillAmount = 1 - skillHeadShotCoolTimer / skillHeadShotCoolTime;
            textHeadShotCoolTime.text = skillHeadShotCoolTimer.ToString("F1");
            textHeadShotCoolTime.enabled = true;
        }
        if (skillHeadShotCoolTimer == 0)
        {
            textHeadShotCoolTime.enabled = false;
        }

        if (skillTenCoolTimer > 0)
        {
            skillTenCoolTimer -= Time.deltaTime;
            if (skillTenCoolTimer < 0)
            {
                skillTenCoolTimer = 0;
                isTen = false;
            }
            tenImgFill.fillAmount = 1 - skillTenCoolTimer / skillTenCoolTime;
            textTenCoolTime.text = skillTenCoolTimer.ToString("F1");
            textTenCoolTime.enabled = true;
        }
        if (skillTenCoolTimer == 0)
        {
            textTenCoolTime.enabled = false;
        }


        if (skillShotGunCoolTimer > 0)
        {
            skillShotGunCoolTimer -= Time.deltaTime;
            if (skillShotGunCoolTimer < 0)
            {
                skillShotGunCoolTimer = 0;
            }
            shotGunImgFill.fillAmount = 1 - skillShotGunCoolTimer / skillShotGunCoolTime;
            textShotGunCoolTime.text = skillShotGunCoolTimer.ToString("F1");
            textShotGunCoolTime.enabled = true;
        }
        if (skillShotGunCoolTimer == 0)
        {
            textShotGunCoolTime.enabled = false;
        }



    }

    private void initUI()
    {
        headShotImgFill.fillAmount = 1;
        textHeadShotCoolTime.text = "";
        textHeadShotCoolTime.enabled = false;

        tenImgFill.fillAmount = 1;
        textTenCoolTime.text = "";
        textTenCoolTime.enabled = false;

        shotGunImgFill.fillAmount = 1;
        textShotGunCoolTime.text = "";
        textShotGunCoolTime.enabled = false;
    }

    private void headShot()
    {
        if (transform.localScale.x == 1f)
        {
            Instantiate(skillHeadShot, trsAttack.position, angle, dynamicObject);
        }
        else if (transform.localScale.x == -1f)
        {
            Instantiate(skillHeadShot, trsAttack.position, Quaternion.identity, dynamicObject);
        }
    }

    private void Ten()
    {
        if (remainTenSkill <= 0) return;

        //for(int i =0; i < 10; i++)
        //{
        //    if (transform.localScale.x == 1f)
        //    {
        //        Instantiate(skillTen, trsAttack.position, angle, dynamicObject);
        //    }
        //    else if (transform.localScale.x == -1f)
        //    {
        //        Instantiate(skillTen, trsAttack.position, Quaternion.identity, dynamicObject);
        //    }
        //}

        if (isSetTenSkill == false)
        {
            curLookAtPos = transform.localScale.x;
            timerTenSkill = 0.0f;

            isSetTenSkill = true;
        }

        timerTenSkill += Time.deltaTime;
        if (timerTenSkill >= timeTenSkill)
        {
            //if (curLookAtPos == 1f)
            //{
            //    Instantiate(skillTen, trsAttack.position, angle, dynamicObject);
            //}
            //else 
            //{
            //    Instantiate(skillTen, trsAttack.position, Quaternion.identity, dynamicObject);
            //}
            Instantiate(skillTen, trsAttack.position, curLookAtPos == 1f ? angle : Quaternion.identity, dynamicObject);
            timerTenSkill = 0.0f;
            remainTenSkill--;

            if (remainTenSkill <= 0)
            {
                isTen = false;
                isSetTenSkill = false;
            }
        }
    }

    IEnumerator corSkillTen()
    {
        for (int i = 0; i < 10; i++)
        {
            if (transform.localScale.x == 1f)
            {
                Instantiate(skillTen, trsAttack.position, angle, dynamicObject);
            }
            else if (transform.localScale.x == -1f)
            {
                Instantiate(skillTen, trsAttack.position, Quaternion.identity, dynamicObject);
            }
            yield return new WaitForSeconds(timeTenSkill);
        }
    }

    private void ShotGun()
    {
        for (int i = 0; i < 7; ++i)
        {

            if (transform.localScale.x == 1f)
            {
                float z = 145;
                z += i * 15;
                Instantiate(skillShotGun, trsAttack.position, Quaternion.Euler(0, 0, z), dynamicObject);
            }
            else if (transform.localScale.x == -1f)
            {
                float z = -45;
                z += i * 15;
                Instantiate(skillShotGun, trsAttack.position, Quaternion.Euler(0, 0, z), dynamicObject);
            }
        }
    }




}

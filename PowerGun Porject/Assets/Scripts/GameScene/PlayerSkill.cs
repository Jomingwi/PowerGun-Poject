using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    PlayerController playerController;
    GameManager gameManager;
    Quaternion angle = Quaternion.Euler(0, 0, 180);

    [SerializeField] Transform dynamicObject;
    [SerializeField] Transform trsAttack;

    bool skillshot;

    [Header("Çìµå¼¦")]
    [SerializeField] GameObject skillHeadShot;
    [SerializeField] float skillHeadShotCoolTime = 5;
    [SerializeField] Image headShotImgFill;
    [SerializeField] TMP_Text textHeadShotCoolTime;
    float skillHeadShotCoolTimer;
    bool isHeadShot;

    [Header("¿¬»ç")]
    [SerializeField] GameObject skillTen;
    [SerializeField] float skillTenCoolTime = 5;
    [SerializeField] Image tenImgFill;
    [SerializeField] TMP_Text textTenCoolTime;
    float skillTenCoolTimer;
    bool isTen;

    [Header("¼¦°Ç")]
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
                enemy.Hit(15);
            }
            if (isTen == true)
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Hit(4);
            }
            if (isShotGun == true)
            {
                Destroy(gameObject);
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Hit(3);
            }

        }
        if (skillshot == true && collision.tag == "Player")
        {
            Destroy(gameObject);
            PlayerController player = collision.GetComponent<PlayerController>();
            player.Hit();
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
    }


    public void skillKeySetting()
    {

        if (Input.GetKey(KeyCode.X) && skillHeadShotCoolTimer == 0)
        {
            isHeadShot = true;
            playerSkill();
            headShot();
        }
        if (Input.GetKey(KeyCode.C) && skillTenCoolTimer == 0)
        {
            isTen = true;
            playerSkill();
            Ten();
        }
        if (Input.GetKey(KeyCode.V) && skillShotGunCoolTimer == 0)
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

        if (isTen == true)
        {
            isTen = false;
            skillTenCoolTimer = skillTenCoolTime;
        }

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
        if(skillHeadShotCoolTimer == 0)
        {
            textHeadShotCoolTime.enabled = false;
        }

        if (skillTenCoolTimer > 0)
        {
            skillTenCoolTimer -= Time.deltaTime;
            if (skillTenCoolTimer < 0)
            {
                skillTenCoolTimer = 0;
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
        for(int i =0; i < 10; i++)
        {
            if (transform.localScale.x == 1f)
            {
                Instantiate(skillTen, trsAttack.position, angle, dynamicObject);
            }
            else if (transform.localScale.x == -1f)
            {
                Instantiate(skillTen, trsAttack.position, Quaternion.identity, dynamicObject);
            }
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

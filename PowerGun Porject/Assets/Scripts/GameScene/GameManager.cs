using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Difficulty curDifficulty;
    GameObject fabExplosion;
    Camera mainCam;

    [Header("����")]
    [SerializeField] List<GameObject> listEnemy;
    [SerializeField] BoxCollider2D boxcoll;

    [Header("�� ����")]
    [SerializeField] Color sliderBossSpawnColor;
    [SerializeField] public Transform trsSpawnPos;
    [SerializeField] Transform trsDynamicObject;
    [SerializeField] bool isSpawn;
    [SerializeField] float enemyMaxSpawnCount = 20;
    float enemySpawnCount;

    [Header("�� ���� UI")]
    [SerializeField] GameObject fabEnemyHP;
    [SerializeField] public Transform trsHpBarPos;
    [SerializeField] Slider slider;
    [SerializeField] Image imgFill;
    [SerializeField] TMP_Text sliderText;
    [SerializeField] public RectTransform trsCanvas;


    [Header("����")]
    [SerializeField] GameObject fabBoss;
    [SerializeField] Transform trsBossPos;
    public Transform TrsBossPos => trsBossPos;




    [Header("���� ����")]
    [SerializeField] GameObject objGameOver;
    [SerializeField] TMP_Text textgameClear;
        



    bool isSpawnBoss = false;
    bool IsSpawnBoss
    {
        set
        {
            isSpawnBoss = value;
        }
    }

    public GameObject FabExplosion
    {
        get { return fabExplosion; }
    }

    public GameObject FabEnemyHp
    {
        get { return fabEnemyHP; }
    }

    PlayerController player;

    public PlayerController Player
    {
        get { return player; }
        set { player = value; }
    }


    private void Awake()
    {
        if (Tool.isStartMainScene == false)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        fabExplosion = Resources.Load<GameObject>("Effect/Explosion");
        fabEnemyHP = Resources.Load<GameObject>("Prefab/fabEnemyHpCanvas");

        initSlider();
        isSpawn = true;
        isSpawnBoss = false;
        enemySpawnCount = 0;
        objGameOver.SetActive(false);

        //���̵� ����
        int difficult = PlayerPrefs.GetInt("DifficultKey", 1);
        curDifficulty = (Difficulty)difficult;
    }

    private void Start()
    {
        mainCam = Camera.main;
        enemyCreate();
    }


    public void enemyCreate()
    {

        if (isSpawn == false) { return; }
        for (int i = 0; i < enemyMaxSpawnCount; i++)
        {
            if (enemySpawnCount < enemyMaxSpawnCount)
            {
                int count = listEnemy.Count;
                int iRand = Random.Range(0, count);

                Vector2 defaultPos = trsSpawnPos.position;
                float x = Random.Range(boxcoll.bounds.min.x, boxcoll.bounds.max.x);
                float y = Random.Range(boxcoll.bounds.min.y, boxcoll.bounds.max.y);
                defaultPos.x = x;
                defaultPos.y = y;

                GameObject go = Instantiate(listEnemy[iRand], defaultPos, Quaternion.identity, trsSpawnPos);
                Enemy goSc = go.GetComponent<Enemy>();
                goSc.difficultyHp(curDifficulty);
                goSc.setHpBar();

                enemySpawnCount++;
                modifySlider();
            }
        }


        if (enemySpawnCount >= enemyMaxSpawnCount)
        {
            isSpawn = false;
        }

    }

    public void SetPlayerHp(float _maxHp, float _curHp)
    {
        player.SetPlayerHp(_maxHp, _curHp);
    }


    /// <summary>
    /// ��ü ų���� ���̳ʽ���
    /// </summary>
    public void enemyKillCount()
    {
        --enemySpawnCount;
        modifySlider();
        bossSpawn();
    }

    private void initSlider()
    {
        slider.minValue = 0;
        slider.maxValue = enemyMaxSpawnCount;
        slider.value = 0;
        sliderText.text = $"{(int)enemySpawnCount} / {(int)enemyMaxSpawnCount}";
        modifySlider();
    }

    public void modifySlider()
    {
        slider.value = enemySpawnCount;
        sliderText.text = $"{enemySpawnCount} / {enemyMaxSpawnCount}";
    }



    public void bossSpawn()
    {
        if (isSpawnBoss == false && enemySpawnCount == 0)
        {
            isSpawn = false;
            IsSpawnBoss = true;

            bossCreate();
        }
    }


    public void bossCreate()
    {
        if (isSpawnBoss == true)
        {
            GameObject go = Instantiate(fabBoss, trsBossPos.transform.position, Quaternion.identity, trsSpawnPos);
            EnemyBoss goSc = go.GetComponent<EnemyBoss>();
            goSc.difficultyHp(curDifficulty);
            goSc.nextPattern(curDifficulty);
            bossinitSlider(goSc.bossMaxHp);
        }
    }


    /// <summary>
    /// ������ ������������ �ִ� ü�°�
    /// </summary>
    /// <param name="maxHP"></param>
    private void bossinitSlider(float maxHP)
    {
        slider.minValue = 0;
        slider.maxValue = maxHP;
        slider.value = maxHP;
        sliderText.text = $"{(int)maxHP}";
        imgFill.color = sliderBossSpawnColor;

    }


    /// <summary>
    /// ������ ü���� ������Ʈ�Ҽ��ֵ��� ���ʹ� ���� hit�� ����Ǵ°�
    /// </summary>
    /// <param name="hp"></param>
    public void bossModifySlider(float hp)
    {
        slider.value = hp;
        sliderText.text = $"{(int)hp}";
    }


    public void GameOver()
    {
        objGameOver.SetActive(true);
        Invoke("gameOverScreen", 5);
    }

    private void gameOverScreen()
    {
        objGameOver.SetActive(false);
        SceneManager.LoadScene(0);
    }
}





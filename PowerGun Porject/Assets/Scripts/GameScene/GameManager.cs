using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Difficulty curDifficulty;
    GameObject fabExplosion;
    Camera mainCam;

    [Header("적기")]
    [SerializeField] List<GameObject> listEnemy;
    [SerializeField] BoxCollider2D boxcoll;

    [Header("적 생성")]
    [SerializeField] Color sliderBossSpawnColor;
    [SerializeField] public Transform trsSpawnPos;
    [SerializeField] Transform trsDynamicObject;
    [SerializeField] bool isSpawn;
    [SerializeField] float enemyMaxSpawnCount = 20;
    float enemySpawnCount;

    [Header("적 생성 UI")]
    [SerializeField] GameObject fabEnemyHP;
    [SerializeField] public Transform trsHpBarPos;
    [SerializeField] Slider slider;
    [SerializeField] Image imgFill;
    [SerializeField] TMP_Text sliderText;
    [SerializeField] public RectTransform trsCanvas;


    [Header("보스")]
    [SerializeField] GameObject fabBoss;
    [SerializeField] Transform trsBossPos;
    public Transform TrsBossPos => trsBossPos;

    [Header("게임 오버")]
    [SerializeField] GameObject objGameClear;
    [SerializeField] TMP_Text textgameClear;

    [SerializeField] GameObject objGameOver;
    [SerializeField] TMP_Text textGameOver;


    [Header("게임 시작")]
    [SerializeField] TMP_Text textGameStart;



    


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
       

        //난이도 설정
        int difficult = PlayerPrefs.GetInt("DifficultKey", 1);
        curDifficulty = (Difficulty)difficult;

        
    }

    private void difficultySpawnCount(Difficulty difficulty)
    {
        if (difficulty == Difficulty.Easy) { enemyMaxSpawnCount = 10; }
        if (difficulty == Difficulty.Hard) { enemyMaxSpawnCount = 30; }
    }

    private void Start()
    {
        mainCam = Camera.main;
        enemyCreate();
        StartCoroutine(doStartText());
    }

    IEnumerator doStartText()
    {
        Color color = textGameStart.color;
        color.a = 0f;
        textGameStart.color = color;

        while (true)
        {
            color = textGameStart.color;
            color.a += Time.deltaTime;
            if (color.a > 1.0f)
            {
                color.a = 1.0f;
            }

            textGameStart.color = color;
            if (color.a == 1.0f)
            {
                break;
            }
            yield return null;

        }

        while (true)
        {
            color = textGameStart.color;
            color.a -= Time.deltaTime;
            if (color.a < 0.0f)
            {
                color.a = 0.0f;
            }

            textGameStart.color = color;
            if (color.a == 0.0f)
            {
                break;
            }
            yield return null;
        }

        Destroy(textGameStart.gameObject);
    }



  

    public void enemyCreate()
    {

        if (isSpawn == false) { return; }
        difficultySpawnCount(curDifficulty);

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
    /// 전체 킬에서 마이너스됨
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
    /// 보스가 생성됬을때의 최대 체력값
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
    /// 보스의 체력을 업데이트할수있도록 에너미 보스 hit에 참고되는값
    /// </summary>
    /// <param name="hp"></param>
    public void bossModifySlider(float hp)
    {
        slider.value = hp;
        sliderText.text = $"{(int)hp}";
    }


    public void GameClear()
    {
        objGameClear.SetActive(true);
        Invoke("gameClearScreen", 5);
    }

    public void GameOver()
    {
        objGameOver.SetActive(true);
        Invoke("gameOverScreen", 5);
    }

    private void gameClearScreen()
    {
        objGameClear.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void gameOverScreen()
    {
        objGameOver.SetActive(false);
        SceneManager.LoadScene(0);
    }

   

  

}





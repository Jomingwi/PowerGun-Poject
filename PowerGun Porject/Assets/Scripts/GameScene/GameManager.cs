using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.Experimental.Playables;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameObject fabExplosion;
    Camera mainCam;

    [Header("적기")]
    [SerializeField] List<GameObject> listEnemy;
    [SerializeField] BoxCollider2D boxcoll;

    [Header("적 생성")]
    [SerializeField] EnemyHp enemyhp;
    [SerializeField] bool isSpawn;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text sliderText;
    [SerializeField] Transform trsSpawnPos;
    [SerializeField] Transform trsDynamicObject;
    [SerializeField] float enemyMaxSpawnCount = 20;
    float enemySpawnCount;


    [Header("보스")]
    [SerializeField] bool isSpawnBoss;



    public GameObject FabExplosion
    {
        get { return fabExplosion; }
    }



    PlayerController player;

    public PlayerController Player
    {
        get { return player; }
        set { player = value; }
    }

    Enemy enemy;

    public Enemy Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }


    MapBound map;

    public MapBound Map
    {
        get { return map; }
        set { map = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        fabExplosion = Resources.Load<GameObject>("Effect/Explosion");
        initSlider();
        isSpawn = true;

    }

    private void Start()
    {
        mainCam = Camera.main;
        enemyCreate();

    }

    private void Update()
    {

    }



    public void enemyCreate()
    {

        if (isSpawn == false) { return; }
        for (int i = 0; i < enemyMaxSpawnCount; i++)
        { 
            if (enemySpawnCount <= enemyMaxSpawnCount)
            {
                int count = listEnemy.Count;
                int iRand = Random.Range(0, count);

                Vector2 defaultPos = trsSpawnPos.position;
                float x = Random.Range(boxcoll.bounds.min.x, boxcoll.bounds.max.x);
                float y = Random.Range(boxcoll.bounds.min.y, boxcoll.bounds.max.y);
                defaultPos.x = x;
                defaultPos.y = y;

                GameObject go = Instantiate(listEnemy[iRand], defaultPos, Quaternion.identity, trsSpawnPos);
                EnemyHp goSc = go.GetComponent<EnemyHp>();
                enemySpawnCount++;

            }
        }

        if (enemySpawnCount == enemyMaxSpawnCount)
        {
            isSpawn = false;
        }

    }

    public void EnemyHpBar()
    {
        int count = listEnemy.Count;
        for(int i =0; i < count; i++)
        {
            Instantiate(listEnemy[i], enemy.transform.position, Quaternion.identity, transform.parent);
        }
    }

    public void SetPlayerHp(float _maxHp, float _curHp)
    {
        player.SetPlayerHp(_maxHp, _curHp);
    }

    public void SetEnemyHp(float _maxHp, float _curHp)
    {
        enemyhp.SetEnemyHp(_maxHp, _curHp);
    }




    /// <summary>
    /// 전체 킬에서 마이너스됨
    /// </summary>
    public void enemyKillCount()
    {
        enemySpawnCount--;
    }

    private void initSlider()
    {
        slider.maxValue = enemyMaxSpawnCount;
        sliderText.text = $"{(int)enemySpawnCount} / {(int)enemyMaxSpawnCount}";
        modifySlider();
    }

    public void modifySlider()
    {
        slider.value = enemySpawnCount;
        if (enemySpawnCount == 0)
        {
            isSpawnBoss = true;
        }

    }

    /// <summary>
    /// 포지션은 기본값으로 잡고 적이 없으면 false로 리턴하고 적이 생성되어 있으면 포지션에 에너미 포지션을 넣어준다음 true를 리턴
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public bool EnemyPos(out Vector2 _pos)
    {
        _pos = default;
        if (enemy == null) { return false; }
        else
        {
            _pos = enemy.transform.position;
            return true;
        }
    }
    public bool GetPlayerPos(out Vector3 pos)
    {
        pos = default;
        if (player == null)
        {
            return false;
        }
        else
        {
            pos = player.transform.position;
            return true;
        }
    }

}

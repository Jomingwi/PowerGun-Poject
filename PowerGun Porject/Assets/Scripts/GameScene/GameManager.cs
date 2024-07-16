using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameObject fabExplosion;
    Camera mainCam;

    [Header("적기")]
    [SerializeField] List<GameObject> listEnemy;


    [Header("적 생성")]
    [SerializeField] bool isSpawn;
    [SerializeField] Transform trsSpawnPos;
    [SerializeField] Transform trsDynamicObject;
    [SerializeField] float enemyMaxSpawnCount = 20;
    float enemySpawnCount = 0f;


    [Header("적 체력 게이지")]
    [SerializeField] GameHp gameHP;
    [SerializeField] Slider slider;
    [SerializeField] Image imgEnemyFillSlider;
    [SerializeField] TMP_Text enemyKillCountText;
    [SerializeField] TMP_Text enemyMaxKillCountText;



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
        set {  map = value; }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
        fabExplosion = Resources.Load<GameObject>("Effect/Explosion");
        isSpawn = true;

        initSlider();
        spawnCheck();
    }

    private void Start()
    {
        mainCam = Camera.main;
        enemyCreate();
    }

    private void Update()
    {
       
    }
    private void spawnCheck()
    {
        if(isSpawn == true)
        {
            enemyCreate();
        }
    }


    public void enemyCreate()
    {
        if (isSpawn == false) { return; }
        if(enemySpawnCount < enemyMaxSpawnCount)
        {
            enemySpawnCount = enemyMaxSpawnCount;

            int count = listEnemy.Count;
            int iRand = Random.Range(0, count);

            Vector2 defaultPos = trsSpawnPos.position;
            float x = Random.Range(map.curBound.min.x, map.curBound.max.x);
            float y = Random.Range(map.curBound.min.y, map.curBound.max.y);
            defaultPos.x = x;
            defaultPos.y = y;

            GameObject go = Instantiate(listEnemy[iRand], defaultPos, Quaternion.identity, trsDynamicObject);
            GameHp goSc = go.GetComponent<GameHp>();
            goSc.chaseEnemy();

            if(defaultPos.y < player.transform.position.y && defaultPos.x < player.transform.position.x)
            {
                defaultPos.y = player.transform.position.y;
                defaultPos.x *= -player.transform.localScale.x;
            }

           
        }
        
    }

    public void SetHp(float _maxHp , float _curHp)
    {
        gameHP.SetHp(_maxHp, _curHp);
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
        enemySpawnCount = enemyMaxSpawnCount;
        slider.minValue = 0;
        slider.maxValue = enemyMaxSpawnCount;
        slider.value = 0;
        modifySlider();
    }

    public void modifySlider()
    {
        slider.value = enemySpawnCount;
        enemyKillCountText.text = $"{enemySpawnCount.ToString("d2")} / {enemyMaxSpawnCount.ToString("d2")}";
    }

    /// <summary>
    /// 포지션은 기본값으로 잡고 적이 없으면 false로 리턴하고 적이 생성되어 있으면 포지션에 에너미 포지션을 넣어준다음 true를 리턴
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public bool EnemyPos(out Vector2 _pos)
    {
        _pos = default;
        if(enemy == null) { return false; }
        else
        {
            _pos = enemy.transform.position;
            return true;
        }
    }
    


    public void PlayerPos(Vector2 pos)
    {
        if(player == null) { return; }
    }

    



}

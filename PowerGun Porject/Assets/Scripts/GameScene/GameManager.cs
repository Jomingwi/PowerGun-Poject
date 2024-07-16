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
    



    [Header("적기")]
    [SerializeField] List<GameObject> listEnemy;


    [Header("적 생성")]
    [SerializeField] bool isSpawn;
    [SerializeField] Transform trsSpawnPos;
    [SerializeField] Transform trsDynamicObject;
    float enemyMaxSpawnCount;
    float enemySpawnCount;


    [Header("적 체력 게이지")]
    [SerializeField] GameHp gameHP;
    [SerializeField] Slider Slider;
    [SerializeField] Image imgEnemyFillSlider;



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

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        enemyCreate();
    }




    public void enemyCreate()
    {
        if (isSpawn == false) { return; }
        if(enemySpawnCount < enemyMaxSpawnCount && isSpawn == true)
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

    public void enemyKillCount()
    {
        enemySpawnCount--;
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

    private void enemySlider()
    {
        Slider.minValue = enemySpawnCount;
        Slider.maxValue = enemyMaxSpawnCount;

        enemySpawnCount = enemyMaxSpawnCount;
    }



}

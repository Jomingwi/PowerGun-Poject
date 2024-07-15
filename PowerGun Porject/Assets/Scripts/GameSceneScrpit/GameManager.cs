using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameObject fabExplosion;
    



    [Header("적기")]
    [SerializeField] List<GameObject> listEnemy;


    [Header("적 생성")]
    [SerializeField] bool isSpawn;
    [SerializeField] float enemySpawnCount;
    [SerializeField] Transform trsSpawnPos;
    [SerializeField] Transform trsDynamicObject;

    [Header("체력 게이지")]
    [SerializeField] GameHp gameHP;

    
    


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
        enemyCreate();
    }

    private void Update()
    {
        
    }




    public void enemyCreate()
    {
        if (isSpawn == false) { return; }
        if(enemySpawnCount < 20)
        {
            isSpawn = true;

            int count = listEnemy.Count;
            int iRand = Random.Range(0, count);

            Vector2 defaultPos = trsSpawnPos.position;
            float x = Random.Range(map.curBound.min.x, map.curBound.max.x);
            float y = Random.Range(map.curBound.min.y, map.curBound.max.y);
            defaultPos.x = x;
            defaultPos.y = y;

            GameObject go = Instantiate(listEnemy[iRand], defaultPos, Quaternion.identity, trsDynamicObject);
            GameHp goSc = go.GetComponent<GameHp>();
            goSc.

            if(defaultPos.y < player.transform.position.y && defaultPos.x < player.transform.position.x)
            {
                defaultPos.y = player.transform.position.y;
                defaultPos.x *= -player.transform.localScale.x;         
            }
        }
        isSpawn = false;
    }

    private void modifySlider()
    {

    }


    public void enemyKillCount()
    {
        enemySpawnCount--;
    }

   

    


    public void PlayerPos(Vector2 pos)
    {
        if(player == null) { return; }
    }


}

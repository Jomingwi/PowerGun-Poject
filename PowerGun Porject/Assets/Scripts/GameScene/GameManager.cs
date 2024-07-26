using System.Collections;
using System.Collections.Generic;
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
	[SerializeField] BoxCollider2D boxcoll;

	[Header("적 생성")]
	[SerializeField] public Transform trsSpawnPos;
	[SerializeField] Transform trsDynamicObject;
	[SerializeField] bool isSpawn;
	[SerializeField] float enemyMaxSpawnCount = 20;
	float enemySpawnCount;

	[Header("적 생성 UI")]
	[SerializeField] GameObject fabEnemyHP;
	[SerializeField] public Transform trsHpBarPos;
	[SerializeField] Slider slider;
	[SerializeField] TMP_Text sliderText;
	[SerializeField] public RectTransform trsCanvas;


	[Header("보스")]
	[SerializeField] public bool isSpawnBoss = false;
	[SerializeField] GameObject EnemyBoss;
	[SerializeField] Transform trsBossPos;
	public Transform TrsBossPos => trsBossPos;



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
		if (isSpawnBoss = false && enemySpawnCount == 0)
		{
			isSpawnBoss = true;
			bossCreate();
		}
		else return;
		
	}


	public void bossCreate()
	{
		if (isSpawnBoss == true)
		{
			GameObject go = Instantiate(EnemyBoss, trsBossPos.transform.position, Quaternion.identity, transform.parent);
			EnemyBoss goSc = go.GetComponent<EnemyBoss>();
			bossinitSlider(goSc.bossMaxHp);
		}
	}


	/// <summary>
	/// 보스가 생성됬을때의 최대 체력값
	/// </summary>
	/// <param name="maxHP"></param>
	private void bossinitSlider(float maxHP)
	{
		slider.minValue = maxHP;
		slider.maxValue = maxHP;
		sliderText.text = $"{maxHP}";
	}


	/// <summary>
	/// 보스의 체력을 업데이트할수있도록 에너미 보스 hit에 참고되는값
	/// </summary>
	/// <param name="hp"></param>
	public void bossModifySlider(float hp)
	{
		slider.value = hp;
		sliderText.text = $"{hp / slider.maxValue}";
	}




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
	GameManager gameManager;
	[SerializeField] Image img;


	private void Awake()
	{
		initHp();
	}
	void Start()
	{
		gameManager = GameManager.Instance;
	}

	private void Update()
	{
		ChaseEnemy();
		enemyDie();
	}


	private void initHp()
	{
		img.fillAmount = 1;
	}

	public void ChaseEnemy()
	{
		if (gameManager.GetEnemyPos(out Vector2 pos) == true)
		{
			pos = transform.position;
			pos.y += 5;
			transform.position = pos;
		}
	}


	public void SetEnemyHp(float maxHp, float curHp)
	{
		img.fillAmount = curHp / maxHp;
	}

	private void enemyDie()
	{

		if (img.fillAmount <= 0)
		{
			Destroy(gameObject);
		}
	}


}

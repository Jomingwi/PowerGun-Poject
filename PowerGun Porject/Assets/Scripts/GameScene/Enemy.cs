using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public enum eEnemyType
	{
		EnemyFly,
		EnemyWalk,
		EnemyDragon,
		EnemyBoss,
	}


	[Header("적 설정")]
	[SerializeField] eEnemyType enemyType;

	[SerializeField] float moveSpeed;
	[SerializeField] public float enemyCurHp;
	[SerializeField] public float enemyMaxHP;
	[SerializeField] float moveTimer = 0;
	float moveTime = 3;
	bool isGround;
	float verticalVelocity;


	GameObject fabExplosion;
	GameManager gameManager;
	SpriteRenderer spriteRenderer;
	EnemyHp enemyHP;
	Transform playerPos;

	Rigidbody2D rigid;
	BoxCollider2D boxcoll;
	Vector3 moveDir = new Vector2(1, 0);
	Camera mainCam;

    public void difficultyHp(Difficulty difficulty)
    {
       if(difficulty == Difficulty.Easy) { enemyMaxHP *= 0.7f; }
       if(difficulty == Difficulty.Hard) { enemyMaxHP *= 1.5f; }

		enemyCurHp = enemyMaxHP;
    }


    private void Awake()
	{
		spriteRenderer = transform.GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		boxcoll = GetComponentInChildren<BoxCollider2D>();
		moveTimer = moveTime;
        enemyCurHp = enemyMaxHP;
    }



	private void Start()
	{
		gameManager = GameManager.Instance;
		fabExplosion = gameManager.FabExplosion;
		mainCam = Camera.main;
		playerPos = gameManager.Player.transform;
		setHpBar();

	}


	private void Update()
	{
		if(gameManager.Player == null) { return; }
		enemyGravityCheck();
		moving();
	}

	
	private void moving()
	{
		if (enemyInView() == true)
		{
			moveToPlayer();
		}
		else
		{
			enemyMoving();
		}
	}


	/// <summary>
	/// 에너미가 카메라 안에 들어오면 true
	/// </summary>
	/// <returns></returns>
	private bool enemyInView()
	{
		Vector3 viewPortPoint = mainCam.WorldToViewportPoint(transform.position);
		return viewPortPoint.x > 0 && viewPortPoint.x < 1 && viewPortPoint.y > 0 && viewPortPoint.y < 1;
	}

	/// <summary>
	/// 에너미가 플레이어 포지션쪽으로 움직임
	/// </summary>
	private void moveToPlayer()
	{
		Vector3 distance = playerPos.position - transform.position;
		distance.x = distance.x < 0 ? -1 : 1;
		
		 rigid.velocity = new Vector2(distance.x * moveSpeed, rigid.velocity.y);
       

		Vector3 scale = transform.localScale;
		scale.x = playerPos.position.x < transform.position.x ? -1 : 1;
		transform.localScale = scale;
	}



	public void setHpBar()
	{
		if (gameManager == null) { return; }

		GameObject go = Instantiate(gameManager.FabEnemyHp, gameManager.trsCanvas.transform.position, Quaternion.identity, gameManager.trsHpBarPos); ;
		enemyHP = go.GetComponent<EnemyHp>();
		enemyHP.SetEnemy(this);
	}


	public void Hit(float damage)
	{
		enemyCurHp -= damage;
		enemyHP.SetEnemyHp(enemyMaxHP, enemyCurHp);

		if (enemyCurHp <= 0f)
		{
			Destroy(gameObject);


			GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
			Explosion goSc = go.GetComponent<Explosion>();
			goSc.ImageSize(spriteRenderer.sprite.rect.width);

			gameManager.enemyKillCount();
		}

	}


	private void enemyGravityCheck()
	{
		if (isGround == false)
		{
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
			if (verticalVelocity < -10)
			{
				verticalVelocity = -10;
			}
		}
		else if (isGround == true)
		{
			verticalVelocity = 0;
		}
	}


	/// <summary>
	/// 에너미가 움직이는 코드
	/// </summary>
	private void enemyMoving()
	{
		moveTimer -= Time.deltaTime;
		if (moveTimer < 0f)
		{
			enemyScale();
			moveTimer = moveTime;
		}
		if (boxcoll.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
		{
			enemyScale();
		}
		else if (boxcoll.IsTouchingLayers(LayerMask.GetMask("Ground")) == true)
		{
			isGround = true;
		}
		rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
	}

	/// <summary>
	/// 방향을 반대로 돌림
	/// </summary>
	private void enemyScale()
	{
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

		moveDir.x *= -1;
	}

}
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
	[Header("보스 설정")]
	[SerializeField] float moveSpeed;
	[SerializeField] public float bossMaxHp;
	[SerializeField] public float bossCurHp;
	[SerializeField] float groundCheckLength;
	Transform trsBossPos;
	float timer = 0;


	[SerializeField] bool isGround = false;
	bool isbossMoving = false;
	bool isbossPatternChange = false;
	Vector2 moveDir = new Vector2(1, 0);

	[Header("돌진")]
	[SerializeField] int dashSpeed;
	bool isbossDash;

	[Header("무기 던지기")]
	[SerializeField] Vector2 throwForce;
	[SerializeField] GameObject objThrowWeapon;
	[SerializeField] Transform trsWeapon;


	[Header("점프")]
	float verticalVelocity = 0;
	[SerializeField] int jumpForce;
	bool isJump = false;


	[Header("플레이어 기절")]
	[SerializeField] float playerStunTime;
	[SerializeField] float playerStunTimer;
	bool isStun = false;



	GameObject fabExplosion;
	GameManager gameManager;
	Animator anim;
	Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;
	BoxCollider2D boxcoll;

	Difficulty difficulty;
	int curPattern = 0;
	


	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		boxcoll = GetComponent<BoxCollider2D>();
		spriteRenderer = transform.GetComponent<SpriteRenderer>();
		bossCurHp = bossMaxHp;
	}



	void Start()
	{
		gameManager = GameManager.Instance;
		trsBossPos = gameManager.TrsBossPos;
		fabExplosion = gameManager.FabExplosion;
	}




	void Update()
	{
		if (gameObject == null) return;
		bossGravityCheck();
		bossGroundCheck();

	

		bossMoving();

		doAnim();
	}

	private void bossPattern()
	{
		switch (difficulty)
		{
			case Difficulty.Easy:
				curPattern = Random.Range(0, 1);
				break;
			case Difficulty.Normal:
				curPattern = Random.Range(0, 2);
				break;
			case Difficulty.Hard:
				curPattern = Random.Range(0, 3);
				break;
		}
		
	}

	private void executePattern()
	{
		if (timer < 1)
		{
			timer += Time.deltaTime;
			if (curPattern == 0) { bossDash(); }
			if (curPattern == 1) { createWeapon(); }
			if (curPattern == 2) { jump(); }
			if (curPattern == 3) { Getstun(); }
		}

		if(timer >= 1)
		{
			timer = 0;
		}
		
	}
		
	




	/// <summary>
	///보스가 돌진
	/// </summary>
	private void bossDash()
	{
		
			Vector3 playerPos = gameManager.Player.transform.position;
			Vector3 distance = playerPos - transform.position;
			distance.x = distance.x < 0 ? -1 : 1;

			rigid.velocity = new Vector2(distance.x * dashSpeed, rigid.velocity.y);

			Vector3 scale = transform.localScale;
			scale.x = playerPos.x < transform.position.x ? -1 : 1;
			transform.localScale = scale;
		
	}

	private void createWeapon()
	{
		

			GameObject go = Instantiate(objThrowWeapon, trsWeapon.position, trsWeapon.rotation, gameManager.trsSpawnPos);
			EnemyThowWeapon goSc = go.GetComponent<EnemyThowWeapon>();
			bool isRight = transform.localScale.x < 0 ? true : false;
			Vector2 fixedThrowforce = throwForce;
			if (isRight == false)
			{
				fixedThrowforce = -throwForce;
			}
			goSc.SetForce(trsWeapon.rotation * fixedThrowforce, isRight);
		

	}

	private void jump()
	{
			Vector3 playerPos = gameManager.Player.transform.position;
			Vector3 distance = playerPos - transform.position;
			distance.x = distance.x < 0 ? -1 : 1;

			rigid.velocity = new Vector2(distance.x * moveSpeed, jumpForce);

			Vector3 scale = transform.localScale;
			scale.x = playerPos.x < transform.position.x ? -1 : 1;
			transform.localScale = scale;
		
	}

	public void Getstun()
	{
		
	}




	private void bossGroundCheck()
	{
		isGround = false;
		if (verticalVelocity > 0) { return; }

		RaycastHit2D hit =
			Physics2D.BoxCast(boxcoll.bounds.center, boxcoll.bounds.size, 0, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

		if (hit)
		{
			isGround = true;
		}
	}

	private void bossGravityCheck()
	{
		if (isGround == false)
		{
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
			if (verticalVelocity < -10)
			{
				verticalVelocity = -10;
			}
		}
		else
		{
			verticalVelocity = 0;
		}
	}





	private void bossMoving()
	{
		if (isGround == false) { return; }

		Vector3 playerPos = gameManager.Player.transform.position;
		Vector3 distance = playerPos - transform.position;
		distance.x = distance.x < 0 ? -1 : 1;

		rigid.velocity = new Vector2(distance.x * moveSpeed, rigid.velocity.y);


		Vector3 scale = transform.localScale;
		scale.x = playerPos.x < transform.position.x ? -1 : 1;
		transform.localScale = scale;
	}


	public void bossHit(float damage)
	{

		if (bossCurHp < 0)
		{
			bossCurHp = 0;
		}
		bossCurHp -= damage;
		gameManager.bossModifySlider(bossCurHp);

		if (bossCurHp == 0)
		{
			Destroy(gameObject);

			GameObject go = Instantiate(fabExplosion.gameObject, transform.position, Quaternion.identity, transform.parent);
			Explosion goSc = go.GetComponent<Explosion>();
			goSc.ImageSize(spriteRenderer.sprite.rect.width);
			//게임 클리어 화면이 뜨게 만들어야함
		}

	}



	private void doAnim()
	{
		anim.SetBool("bossMoving", isbossMoving);
		anim.SetBool("bossDash", isbossDash);
	}

}

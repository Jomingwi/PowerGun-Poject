using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
	[Header("���� ����")]
	[SerializeField] float moveSpeed;
	[SerializeField] public float bossMaxHp;
	[SerializeField] public float bossCurHp;
	[SerializeField] float groundCheckLength;
	Transform trsBossPos;
	

	[SerializeField] bool isGround = false;
	bool isbossMoving = false;
	bool isbossPatternChange = false;
	Vector2 moveDir = new Vector2(1, 0);

	[Header("����")]
	[SerializeField] int Pattern;
	[SerializeField] int dashSpeed;
	[SerializeField] float PatternTimer;
	bool isbossDash;

	[Header("���� ������")]
	[SerializeField] int Pattern2;
	[SerializeField] Vector2 throwForce;
	[SerializeField] float pattern2Timer;
	[SerializeField] GameObject objThrowWeapon;
	[SerializeField] Transform trsWeapon;
	

	[Header("����")]
	[SerializeField] int Pattern3;
	[SerializeField] float pattern3Timer;
	float verticalVelocity = 0;

	[Header("�÷��̾� ����")]
	bool palyerStun;
	[SerializeField] float playerStunTime;
	[SerializeField] float playerStunTimer;

	int curPattern = 0;

	GameObject fabExplosion;
	GameManager gameManager;
	Animator anim;
	Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;
	BoxCollider2D boxcoll;

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

	/// <summary>
	///������ ����
	/// </summary>
	private void bossDash()
	{
		if (curPattern == 0)
		{
			Vector3 playerPos = gameManager.Player.transform.position;
			Vector3 distance = playerPos - trsBossPos.position;

			distance.x = distance.x < 0 ? -1 : 1;
			rigid.velocity = new Vector2(distance.x * dashSpeed, rigid.velocity.y);
		}
		curPattern++;
	}

	private void createWeapon()
	{
		if(curPattern == 1)
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
	}




	private void bossGroundCheck()
	{
		isGround = false;
		if(verticalVelocity > 0 ) { return; }

		RaycastHit2D hit = 
			Physics2D.BoxCast(boxcoll.bounds.center, boxcoll.bounds.size, 0, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

		if(hit)
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
		if(isGround == false) { return; } 

		if (isGround == true)
		{
			isbossMoving = true;
			Vector3 playerPos = gameManager.Player.transform.position;
			Vector3 distance = playerPos - transform.position;
			bossMovingCheck();
	
				rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
		}
		

	}

	private void bossMovingCheck()
	{
			Vector2 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
			moveDir.x *= -1;
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
			//���� Ŭ���� ȭ���� �߰� ��������
		}

	}



	private void doAnim()
	{
		anim.SetBool("bossMoving", isbossMoving);
		anim.SetBool("bossDash", isbossDash);
	}






}

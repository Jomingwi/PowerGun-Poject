using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
	[Header("보스 설정")]
	[SerializeField] float moveSpeed;
	[SerializeField] public float bossMaxHp;
	[SerializeField] public float bossCurHp;
	[SerializeField] float groundCheckLength;
	bool isGround = false;
	bool isbossMoving = false;
	bool isbossPatternChange = false;
	Vector2 moveDir = new Vector2(1, 0);

	[Header("돌진")]
	[SerializeField] int Pattern;
	[SerializeField] float PatternTimer;
	bool isbossDash;

	[Header("낫 던지기")]
	[SerializeField] int Pattern2;
	[SerializeField] float pattern2Timer;

	[Header("점프")]
	[SerializeField] int Pattern3;
	[SerializeField] float pattern3Timer;
	float verticalVelocity = 0;

	[Header("플레이어 기절")]
	bool palyerStun;
	[SerializeField] float playerStunTime;
	[SerializeField] float playerStunTimer;

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
		fabExplosion = gameManager.FabExplosion;
	}




	void Update()
	{
		if(gameObject!= null) { return; }
		bossGravityCheck();
		bossGroundCheck();
		
		bossMoving();

		doAnim();
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
		else if(isGround == true)
		{
			verticalVelocity = 0;
		}
	}

	

	

	private void bossMoving()
	{
		if (isGround == true)
		{
			isbossMoving = true;
			Vector3 playerPos = gameManager.Player.transform.position;
			Vector3 distance = playerPos - transform.position;

			transform.position = distance;
			bossScale();
		}
		rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);

	}

	private void bossScale()
	{
		if (gameManager.Player.transform.position.x < transform.position.x || gameManager.Player.transform.position.x > transform.position.x)
		{
			Vector2 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
			moveDir.x *= -1;
		}
		else
		{
			return;
		}
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

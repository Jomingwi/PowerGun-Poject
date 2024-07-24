using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
		[Header("보스 설정")]
		[SerializeField] float moveSpeed;
		[SerializeField] float bossMaxHp;
		[SerializeField] float bossCurHp;
		
		bool isbossMoving = false;
		bool isbossPatternChange = false;
		Vector2 moveDir = new Vector2(1,0);

		[Header("돌진")]
		[SerializeField] int onePattern;
		bool isbossDash;

		[Header("낫 던지기")]
		[SerializeField] int onePattern2;

		[Header("점프")]
		[SerializeField] int onePattern3;

		[Header("플레이어 기절")]
		bool palyerStun;

		GameObject fabExplosion;
		GameManager gameManager;
		Animator anim;
		Rigidbody2D rigid;
		
		

		void Start()
		{
			gameManager = GameManager.Instance;
		}

		


		void Update()
		{
			doAnim();
			bossMoving();
		}


	private void bossMoving()
	{
		isbossMoving = true;
		if(gameManager.Player != null)
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
		if(gameManager.Player.transform.position.x < transform.position.x)
		{
			Vector2 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
			moveDir.x *= -1;
		}
	}
    






	private void doAnim()
	{
		anim.SetBool("boosMoving", isbossMoving);
		anim.SetBool("bossDash", isbossDash);
	}






}

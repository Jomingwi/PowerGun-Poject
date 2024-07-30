using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyThowWeapon : MonoBehaviour
{

    Rigidbody2D rigid;
    Vector2 force;
    bool right;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.Hit(10);
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rigid.AddForce(force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, right == true ? -360f : 360f) * Time.deltaTime);
    }

    public void SetForce(Vector3 _force, bool _isRight)
    {
        force = _force;
        right = _isRight;
    }






}

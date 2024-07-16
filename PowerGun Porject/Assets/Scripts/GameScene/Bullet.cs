using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    bool shoot;


    PlayerController playerController;
    
    

    /// <summary>
    /// ī�޶󿡼� ������� ����
    /// </summary>
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(shoot == false && collision.tag == "Enemy")
        {
            Destroy(gameObject);
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Hit(3);
        }
        if(shoot == true && collision.tag == "Player")
        {
            Destroy(gameObject);
            PlayerController player = collision.GetComponent<PlayerController>();
            player.Hit();
        }
    }



    private void Start()
    {
        
    }


    private void Update()
    {
        bulletMove();
    }

    public void bulletMove()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    public void Shoot()
    {
        shoot = false;
    }

}

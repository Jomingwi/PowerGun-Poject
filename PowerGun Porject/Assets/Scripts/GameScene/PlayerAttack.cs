using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{ 
    [Header("АјАн")]
    [SerializeField] GameObject fabBullet;
    [SerializeField] Transform dynamicObject;
    [SerializeField] Transform trsAttack;


    void Update()
    {
        attack();
    }

    private void attack()
    {
        if(Input.GetKeyDown(KeyCode.Z) == true)
        {
            createAttack();
        }
        
    }




    public void createAttack()
    {
        if (transform.localScale.x == 1f)
        {
            Quaternion angle = Quaternion.Euler(new Vector3(0, 0, 180));
            GameObject go = Instantiate(fabBullet, trsAttack.position, angle, dynamicObject);
            Bullet goSc = go.GetComponent<Bullet>();
            goSc.Shoot();
        }
        else if(transform.localScale.x == -1f)
        {
            Quaternion angle = Quaternion.Euler(new Vector3(0, 0, 0));
            GameObject go = Instantiate(fabBullet, trsAttack.position, angle, dynamicObject);
            Bullet goSc = go.GetComponent<Bullet>();
            goSc.Shoot();
        }
    }
    
}

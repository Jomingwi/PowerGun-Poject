using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum eEnemyType
    {
        EnemyFly,
        EnemyWalk,
        EnemyDragon,
        EnemyBoss,
    }

    [SerializeField] protected eEnemyType enemyType;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float enemyHP;
    protected bool isDie;
    protected GameObject fabExplosion;
    protected GameManager gameManager;
   
   
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum ehitType
    {
        spikeCheck,
        bodyCheck,
    }
    
    PlayerController playerController;
    [SerializeField] ehitType hitType;


    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerController.TriggerEnter(hitType , collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerController.TriggerExit(hitType, collision);
    }


}

   
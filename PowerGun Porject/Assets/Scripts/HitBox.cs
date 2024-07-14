using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum ehitboxType
    {
        SpikeCheck,
    }

    [SerializeField] ehitboxType hitboxType;
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
}

   
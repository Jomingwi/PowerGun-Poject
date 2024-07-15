using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController playerCtrl;
    


    void Start()
    {
        playerCtrl = GetComponent<PlayerController>();
    }

  
    void Update()
    {
        
    }
}

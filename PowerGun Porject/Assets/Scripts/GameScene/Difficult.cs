using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficult : MonoBehaviour
{
    public enum eDifficultType
    {
        easy,
        normal,
        hard,
    }
    [SerializeField] eDifficultType difficultType;

    
}

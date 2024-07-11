using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] BoxCollider2D coll;
    Bounds curBound;
    [SerializeField] Transform trsPlayer;

    void Start()
    {
        mainCam = Camera.main;
        checkBound(); 
    }

    
    void Update()
    {
        if(trsPlayer == null) { return; }

        mainCam.transform.position = new Vector3(
            Mathf.Clamp(trsPlayer.position.x, curBound.min.x, curBound.max.x),
            Mathf.Clamp(trsPlayer.position.y, curBound.min.y, curBound.max.y + 2),
            mainCam.transform.position.z
            ) ;
    }

    private void checkBound()
    {
        float height = mainCam.orthographicSize;
        float width = height * mainCam.aspect; // aspect  => ∫Ò¿≤ width / height

        curBound = coll.bounds;

        float minX = curBound.min.x + width;
        float minY = curBound.min.y + height;

        float maxX = curBound.max.x - width;
        float maxY = curBound.max.y - height;

        curBound.SetMinMax(new Vector3(minX,minY) , new Vector3(maxX,maxY));
    }
}

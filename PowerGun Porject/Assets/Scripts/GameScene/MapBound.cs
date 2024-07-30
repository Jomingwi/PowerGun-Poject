using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapBound : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] public BoxCollider2D coll;
    [SerializeField] public BoxCollider2D bossColl;
    public Bounds curBound;
    [SerializeField] Transform trsPlayer;
    [SerializeField] Transform trsBackGround;

    EnemyBoss EnemyBoss;

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
            Mathf.Clamp(trsPlayer.position.y, curBound.min.y, curBound.max.y),
            mainCam.transform.position.z
            ) ;

        trsBackGround.transform.position = new Vector3(
            mainCam.transform.position.x, trsBackGround.position.y, 0);
    }

    public void checkBound()
    {
        float height = mainCam.orthographicSize;
        float width = height * mainCam.aspect; // aspect  => 비율 width / height

        curBound = coll.bounds;
        if(EnemyBoss != null)
        {
            curBound = bossColl.bounds;
        }

        float minX = curBound.min.x + width;
        float minY = curBound.min.y + height;

        float maxX = curBound.max.x - width;
        float maxY = curBound.max.y - height;

        curBound.SetMinMax(new Vector3(minX,minY) , new Vector3(maxX,maxY)); //최소값과 최대값을 설정할수 있는 함수
    }

}

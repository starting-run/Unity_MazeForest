using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNPC : MonoBehaviour
{
    public  GameObject waypoint;
    public float speed = 2f;

    Vector3[] wayPointList;
    Vector3 targetWayPoint;
    int currentWayPoint = 0;

    void Start () {
        int count = waypoint.transform.childCount;
        wayPointList = new Vector3[count];
        for (int i = 0; i < count; i++)
            wayPointList [i] = waypoint.transform.GetChild (i).transform.position;
        targetWayPoint = wayPointList[0];
    }


    void Update () {
        transform.forward = Vector3.RotateTowards(transform.forward, 
            targetWayPoint - transform.position, speed * Time.deltaTime, 0.0f);

        transform.forward = new Vector3(transform.forward.x,0,transform.forward.z);
        
        
        transform.position = Vector3.MoveTowards (transform.position, targetWayPoint,
            speed * Time.deltaTime);

        float distance = Vector3.Distance (transform.position, targetWayPoint);                
        if (  distance <= 0.5) {
            currentWayPoint++;
            targetWayPoint = wayPointList [currentWayPoint % wayPointList.Length];
        }
    }
}
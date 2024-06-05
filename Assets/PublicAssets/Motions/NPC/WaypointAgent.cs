using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointAgent : MonoBehaviour
{
     public GameObject  path;
    private int destPoint = 0;
    private NavMeshAgent agent;
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        GotoNextPoint();
    }
    void GotoNextPoint() {
        agent.destination = path.transform.GetChild(destPoint).transform.position;
        destPoint = (destPoint + 1) % path.transform.childCount;        
    }
    void Update () {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}

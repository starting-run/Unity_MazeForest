using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public GameObject target;
    Vector3 origin;
    bool isDetected = false;
    int state = 0 ;   // 0 : normal    1 : follow    2:  back to origin


    void Start (){ 
        origin = transform.position;        
    }
    void Update () {
    if ( isDetected ) 
           GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }

    void OnTriggerEnter(Collider col) {

        if ( col.tag == "Player")  {
            isDetected = true;  
            state = 1;
        }
    }

    void OnTriggerExit(Collider col) { 

        if ( col.tag == "Player")  {
            isDetected = false;
            state = 2;
            GetComponent<NavMeshAgent>().SetDestination(origin);
        }
    }
}



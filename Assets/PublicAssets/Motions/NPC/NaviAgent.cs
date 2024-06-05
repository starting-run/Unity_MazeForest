using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NaviAgent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Alpha1) ) 
         GetComponent<NavMeshAgent>().isStopped = true;
         if ( Input.GetKeyDown(KeyCode.Alpha2) ) 
         GetComponent<NavMeshAgent>().isStopped = false;
        
    }
}

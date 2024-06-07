using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AIAgent2 : MonoBehaviour
{
    public enum ActionType { None, Follow, Target, Random, Waypoint};

    public GameObject target;
    public float wait = 1.0f;
    public float wait2 = 2;
    public float range = 10.0f;

    public Transform waypoint; 
    public ActionType actionType = ActionType.None;
    

    private int index = 0;
    private int state = 0; //  // 0 : normal    1 : follow    2:두리번    3:  back to origin
    


    private Vector3 origin;    
    private Coroutine coroutine;
    private IEnumerator[] actions = new IEnumerator[5];
    

    void Start (){ 
        origin = transform.position;
        actions[0] = null;
        actions[1] = MoveFollow();
        actions[2] = MoveTarget();
        actions[3] = MoveRandom();
        actions[4] = MoveWaypoint();
        if ( actionType != ActionType.None ) StartCoroutine(actions[(int)actionType]); 
    }

    void Update () {
        if ( state == 1 )         
           GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        if ( state == 3 ) {           
           if ( Vector3.Distance(origin, transform.position) < 0.3f ) {
                state = 0;
                index = 0;
                if ( actionType != ActionType.None ) StartCoroutine(actions[(int)actionType]); 
           }
        }
    }

    void OnTriggerEnter(Collider col) {
        if ( coroutine != null) StopCoroutine(coroutine);
        state = 1;
    }

    void OnTriggerExit(Collider col) { 
        state = 2;
        StartCoroutine(LookAround());        
    }  

    IEnumerator MoveFollow()
    {
        while ( true ) {
            Vector3 targetPos = target.transform.position;            
            GetComponent<NavMeshAgent>().SetDestination(targetPos);                     
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator MoveTarget()
    {
        while ( true ) {            
            Vector3 targetPos =  target.transform.position;
            GetComponent<NavMeshAgent>().SetDestination(targetPos);                     
            while (   Vector3.Distance(transform.position, targetPos) >= 0.3  )
            {
                yield return new WaitForSeconds(0.1f);
            }            
            yield return new WaitForSeconds(wait);
        }
    }
    IEnumerator MoveRandom()
    {
        while ( true ) {
            float tx = Random.Range(-range, range);
            float ty = Random.Range(-range, range);

            Vector3 targetPos = new Vector3(tx, transform.position.y, ty);   
            GetComponent<NavMeshAgent>().SetDestination(targetPos);                     

            Vector3 old = new Vector3(0,0,1000);             
            while (   Vector3.Distance(transform.position, old) >= 0.00001f   )
            {
                print(Vector3.Distance(transform.position, old));
                old = transform.position;
                yield return new WaitForSeconds(0.1f);
            }            
            yield return new WaitForSeconds(wait);
        }
    }
    
     IEnumerator MoveWaypoint()
    {
        while ( true ) {            
            Vector3 targetPos = waypoint.GetChild(index++ % waypoint.childCount).transform.position;            
            GetComponent<NavMeshAgent>().SetDestination(targetPos);                     

            Vector3 old = new Vector3(0,0,1000);             
            while (   Vector3.Distance(transform.position, old) >= 0.00001f  )
            {
                old = transform.position;
                yield return new WaitForSeconds(0.1f);
            }            
            yield return new WaitForSeconds(wait);
        }
    }

      IEnumerator LookAround() {
        yield return new WaitForSeconds(wait2);
        state = 3;
        GetComponent<NavMeshAgent>().SetDestination(origin);        
    }
}

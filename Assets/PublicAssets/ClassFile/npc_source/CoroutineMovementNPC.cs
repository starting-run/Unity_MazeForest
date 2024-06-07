using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Character;
using UnityEngine;

public class CoroutineMovementNPC : MonoBehaviour
{
    public enum Kind
    {
        ToTarget,
        Random,
        Player
    }


    public Transform target;
    public float duration = 2.0f;
    public float size = 10.0f;
    public float speed = 5.0f;
    public float wait = 1.0f;
    // Start is called before the first frame update

    public Kind  kind = Kind.ToTarget;
    void Start()
    {
        if ( kind == Kind.ToTarget) StartCoroutine(MoveTo());
        if ( kind == Kind.Random) StartCoroutine(MoveRandom());       
        if ( kind == Kind.Player) StartCoroutine(MoveToPlayer());        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     IEnumerator MoveTo()
    {
        Vector3 targetPos = target.position;        
        float elapsedTime = 0f;
        Vector3 startingPos = transform.position;

        while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);
                transform.forward = (targetPos - transform.position).normalized;
                elapsedTime += Time.deltaTime;
                yield return null;
            }        
        transform.position = targetPos;
    }

        IEnumerator MoveRandom()
    {
        while ( true ) {
            float tx = Random.Range(-size, size);
            float ty = Random.Range(-size, size);

            Vector3 targetPos = new Vector3(tx, 0, ty);        
            
            while (  (transform.position - targetPos).magnitude >= 0.1 )
            {
                Vector3 dir =  (targetPos - transform.position).normalized;
                transform.position +=  dir * speed *Time.deltaTime;
                transform.forward = dir;            
                yield return null;
            }            
            yield return new WaitForSeconds(wait);
        }
    }



    IEnumerator MoveToPlayer()
    {
        while ( true ) {
            float tx =  target.position.x;
            float tz =  target.position.z;

            Vector3 targetPos = new Vector3(tx, 0, tz);        
             
            float elapsedTime = 0f;
            while (  elapsedTime < duration )
            {
                Vector3 dir =  (targetPos - transform.position).normalized;
                transform.position +=  dir * speed *Time.deltaTime;
                transform.forward = dir;    
                elapsedTime += Time.deltaTime;        
                yield return null;
            }                 
        }
    }



}

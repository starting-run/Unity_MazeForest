using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNPC : MonoBehaviour
{
    public Transform target;
    public float speed =2f;

    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.RotateTowards(transform.forward,
            target.position - transform.position, speed * Time.deltaTime,0.0f);

        transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z); 

        transform.position = Vector3.MoveTowards(
            transform.position, target.position, speed * Time.deltaTime);         
    }
}

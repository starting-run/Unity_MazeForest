using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBasic : MonoBehaviour
{

    public float speed = 5;
    public float rotSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis, vAxis;
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        Vector3 moveVec= new Vector3(hAxis, 0, vAxis).normalized;        

        // 방법 #1
        //transform.LookAt(transform.position + moveVec);
        //transform.position += moveVec * speed * Time.deltaTime;


        // 방법 #2
        // forward 벡터계산(한번에 방향 전환)
        // if ( moveVec != Vector3.zero) 
        //     transform.forward = moveVec;
        // transform.position += moveVec * speed * Time.deltaTime;

        // 방법 #3
        // forward 벡터계산(보간을 통해 천천히 방향 전환)
        if ( moveVec != Vector3.zero) 
            transform.forward = Vector3.Lerp(transform.forward, moveVec, rotSpeed * Time.deltaTime);
        transform.position += moveVec * speed * Time.deltaTime;

    }
}

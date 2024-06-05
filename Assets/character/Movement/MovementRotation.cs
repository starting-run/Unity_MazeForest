using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRotation : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 10;
        public float rotSpeed = 200;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         float hAxis, vAxis;
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");        
        
        transform.Rotate( Vector3.up * hAxis * rotSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * vAxis * speed * Time.deltaTime);
    }
}

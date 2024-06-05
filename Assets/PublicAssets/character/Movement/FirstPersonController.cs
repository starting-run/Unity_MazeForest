using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
        // Start is called before the first frame update

    public GameObject camera;

    public float speed = 10;
    public float rotSpeed = 200;
    void Start()
    {
    }
    
    private float h = 0.0f;
    private float v = 0.0f;

    void Update()
    {
         float hAxis, vAxis;
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");       

        Vector3 moveVec= new Vector3(hAxis, 0, vAxis).normalized;      
        
        transform.Translate(moveVec * speed * Time.deltaTime);

        h += rotSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
        v -= rotSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
        
        camera.transform.rotation = Quaternion.Euler(v, h, 0);        
        transform.rotation = Quaternion.Euler(0, h, 0);        
        


    }

}

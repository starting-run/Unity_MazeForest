using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour{
    public GameObject target;
    public Vector3 offset = new Vector3(0, 3.28f, -6);
    public float speed = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private float h = 0.0f;
    private float v = 0.0f;

    void Update()
    {
        //transform.LookAt(target.transform.position);

        h += speed * Input.GetAxis("Mouse X") * Time.deltaTime;
        v -= speed * Input.GetAxis("Mouse Y") * Time.deltaTime;
        h = Mathf.Clamp(h, -80, 50);
        transform.rotation = Quaternion.Euler(v, h, 0);

        transform.position = target.transform.position + offset;
    }
}

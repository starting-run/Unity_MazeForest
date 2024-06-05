using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBlock : MonoBehaviour
{
    public static Vector3 ori;

     void OnTriggerEnter(Collider other) 
    {
        ori = transform.position;        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InVisibleBlock : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("Do", 3, 3);
        
    }

    void Do() {

        GetComponent<BoxCollider>().enabled = ! GetComponent<BoxCollider>().enabled;
        GetComponent<MeshRenderer>().enabled = ! GetComponent<MeshRenderer>().enabled;

    }
    
}

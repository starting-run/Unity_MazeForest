using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InVisibleBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Do", 3, 3);
        
    }

    // Update is called once per frame


    void Do() {

        GetComponent<BoxCollider>().enabled = ! GetComponent<BoxCollider>().enabled;
        GetComponent<MeshRenderer>().enabled = ! GetComponent<MeshRenderer>().enabled;

    }
    
}

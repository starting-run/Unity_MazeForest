using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DeathBlock : MonoBehaviour
{    public GameObject player;
    // Start is called before the first frame update
    
   void OnTriggerEnter(Collider other) 
    {
        StartCoroutine("Do");
    }
    IEnumerator Do() {
        player.GetComponent<ThirdPersonController>().enabled = false;       
        player.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(2);
        player.transform.position = CheckPointBlock.ori;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<ThirdPersonController>().enabled = true;        
        player.GetComponent<CharacterController>().enabled = true;

    }

}

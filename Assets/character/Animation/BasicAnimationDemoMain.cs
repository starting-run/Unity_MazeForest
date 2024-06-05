using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimationDemoMain : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private bool IsWalking = false;
    void Update()
    {
        if ( Input.GetKeyDown(  KeyCode.Alpha1)  ) animator.SetTrigger("Attack");
        if ( Input.GetKeyDown(  KeyCode.Alpha2)  ) animator.SetTrigger("Death");
        if ( Input.GetKeyDown(  KeyCode.Alpha3)  ) {
            IsWalking = ! IsWalking;
            animator.SetBool("IsWalking", IsWalking);            
        }

    }
}

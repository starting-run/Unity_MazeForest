using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blend : MonoBehaviour
{
    private Animator animator;
    [Range(0, 1)]
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", speed);
    }
    
}
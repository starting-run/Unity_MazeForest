using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blend : MonoBehaviour
{
    private Animator animator;
    [Range(0, 1)]
    public float speed;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        animator.SetFloat("Speed", speed);
    }
}
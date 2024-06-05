using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent_ : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame

    Vector3 old = Vector3.zero;
    void Update()
    {
        float mag = (transform.position - old).magnitude;

        if (mag == 0) animator.SetFloat("Speed", 0);
        else if (mag < 0.02) animator.SetFloat("Speed", 0.2f);
        else animator.SetFloat("Speed", 0.5f);
        old = transform.position;
    }

}
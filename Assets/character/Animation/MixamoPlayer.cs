using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class MixamoPlayer : MonoBehaviour
    {

        public int speed = 10;
        // Start is called before the first frame update
        private Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();            
        }

        // Update is called once per frame
        void Update()
        {
            float hAxis;
            float vAxis;
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");

            Vector3 moveVec = new Vector3(hAxis, 0, vAxis).normalized;

            if ( moveVec == Vector3.zero) animator.SetFloat("Speed", 0);
            else {            
                if ( Input.GetKey(KeyCode.LeftShift) )
                    animator.SetFloat("Speed", 0.5f);
                else animator.SetFloat("Speed", 0.2f);
                transform.position += moveVec * speed * Time.deltaTime;
                transform.forward = moveVec;
            }
        }
    }

}
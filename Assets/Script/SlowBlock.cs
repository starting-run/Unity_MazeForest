using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBlock : MonoBehaviour
{
    public ThirdPersonController ThirdPersonController;

    private float moveSpeedSave = 0;
    private float sprintSpeedSave = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moveSpeedSave = ThirdPersonController.MoveSpeed;
            sprintSpeedSave = ThirdPersonController.SprintSpeed;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdPersonController.MoveSpeed = 0.5f;
            ThirdPersonController.SprintSpeed = 1.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdPersonController.MoveSpeed = moveSpeedSave;
            ThirdPersonController.SprintSpeed = sprintSpeedSave;
        }
    }
}

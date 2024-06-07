using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 인트로 블렌드 속도 조절
 */

public class MainCamera : MonoBehaviour
{
    CinemachineBrain cinemachineBrain;
    void Start()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        cinemachineBrain.m_DefaultBlend.m_Time = 3f;

        StartCoroutine(DelayedModifyBlendSpeed());
    }

    IEnumerator DelayedModifyBlendSpeed()
    {
        yield return new WaitForSeconds(10);

        cinemachineBrain.m_DefaultBlend.m_Time = 0.3f;
    }
}

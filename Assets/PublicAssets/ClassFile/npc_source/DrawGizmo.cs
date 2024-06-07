using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    void OnDrawGizmos() {
        int  count=  transform.childCount;
        Vector3  pre  =  Vector3.zero;
        Vector3 pos;
        for(int i = 0; i < count;i++) {
            Gizmos.color = i == 0 ? Color.red : Color.black;
            pos = transform.GetChild(i).transform.position;
            Gizmos.DrawWireSphere(pos,1f);
            if( i > 0)Gizmos.DrawLine(pre, pos);
            pre = pos;
        }        
    }
}

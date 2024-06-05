using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 블록을 밟았을 때 처리
public class ScriptBlock : MonoBehaviour
{
    public ScenarioEngine engine;
    public string filename;
    private void OnTriggerEnter(Collider other)
    {
        //if로 플레이어 인지 체크(태그 사용)
        string script = Resources.Load<TextAsset>(filename).ToString();
        StartCoroutine(engine.PlayScript(script));
    }
}

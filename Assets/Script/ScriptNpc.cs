using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 블록을 밟았을 때 처리
public class ScriptNpc : MonoBehaviour
{
    public ScenarioEngine engine;
    public string filename;
    public bool justOneCheck = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && justOneCheck == false)
        {
            string script = Resources.Load<TextAsset>(filename).ToString();
            StartCoroutine(engine.PlayScript(script));
            justOneCheck = true;
        }
    }
}

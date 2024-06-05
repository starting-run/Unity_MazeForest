using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 블록을 밟았을 때 처리
public class ScriptBlock : MonoBehaviour
{
    public ScenarioEngine engine;
    public string filename;
    public string question;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            string script = Resources.Load<TextAsset>(filename).ToString();
            StartCoroutine(engine.PlayScript(script, question));
        }
    }
}

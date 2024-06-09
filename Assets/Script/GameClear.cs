using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 블록을 밟았을 때 처리
public class GameClear : MonoBehaviour
{
    public ScenarioEngine engine;
    public AudioClip congBgm;
    public bool justOneCheck = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && justOneCheck == false)
        {
            GameManager.Instance.PlaySound(congBgm);
            GameManager.Instance.GameClear();
            string script = Resources.Load<TextAsset>("GameClear").ToString();
            StartCoroutine(engine.PlayScript(script));
            justOneCheck = true;
        }
    }
}

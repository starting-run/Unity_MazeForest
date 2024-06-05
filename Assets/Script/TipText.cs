using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class TipText : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private string[] TipTexts = new string[] { "첫번째 팁입니다.",
        "두번째 팁입니다.",
        "세번째 팁입니다.",
        "네번째 팁입니다."};
    private int RandomNum;

    void OnEnable()
    {
        RandomNum = Random.Range(0, 3);
    }

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = TipTexts[RandomNum];
    }



    // Update is called once per frame
    void Update()
    {

    }
}

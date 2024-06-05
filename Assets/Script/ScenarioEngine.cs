using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using Cinemachine;
using System;

public class ScenarioEngine : MonoBehaviour
{
    // Start is called before the first frame update

    public Canvas canvas;
    public GameObject[] cameras;

    public string[] objectsName;



    //private Image back;
    private Image front;
    private RawImage video;
    private TextMeshProUGUI text;
    private Canvas dialog;
    private VideoPlayer player;

    private Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        }

        dialog = canvas.transform.Find("Dialog").GetComponent<Canvas>();
        if (objectsName.Length > 0)
        {
            foreach (string name in objectsName) map.Add(name, GameObject.Find(name));
        }

        /* 스크립트 이름을 인자로 받는 경우 이 부분은 주석처리 
        string script = Resources.Load<TextAsset>("script0").ToString();
        StartCoroutine(PlayScript(script));*/
        /* 스크립트 이름을 인자로 받는 경우 이 부분은 주석처리 */

    }

    public IEnumerator PlayScript(string script, string question = "")
    {
        yield return StartCoroutine(FadeCanvas(true, 0f)); // Fade In
        foreach (string token in script.Split('\n'))
        {
            string[] tokens = Parsing(token);
            string fun = tokens[0];
            if (fun == "video")
            {
                string clip = tokens[1];
                front.enabled = false;
                dialog.enabled = false;
                video.enabled = true;
                player.clip = Resources.Load<VideoClip>(clip);
                float len = (float)player.clip.length;
                player.Play();
                yield return new WaitForSeconds(len);
            }
            else if (fun == "text")
            {
                text.enabled = true;
                text.text = tokens[1];
                yield return new WaitForSeconds(float.Parse(tokens[2] == "" ? "0" : tokens[2]));
            }
            else if (fun == "image")
            {
                front.enabled = true;
                video.enabled = false;
                dialog.enabled = false;
                front.sprite = Resources.Load<Sprite>(tokens[1]);
                yield return new WaitForSeconds(float.Parse(tokens[2] == "" ? "0" : tokens[2]));
            }
            else if (fun == "wait")
            {
                yield return new WaitForSeconds(float.Parse(tokens[1] == "" ? "2" : tokens[1]));
            }
            else if (fun == "dialog")
            {
                dialog.enabled = true;
                dialog.transform.Find("Question_Image").GetComponent<Image>().enabled = false;
                //front.enabled = false;
                //video.enabled = false;
                string ch = tokens[1];
                string text = tokens[2];
                if (ch != "null")
                {
                    dialog.transform.Find("Image").GetComponent<Image>().enabled = true;
                    dialog.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ch);
                    dialog.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
                }
                else
                {
                    dialog.transform.Find("Image").GetComponent<Image>().enabled = false;
                    dialog.transform.Find("Text_Center").GetComponent<TextMeshProUGUI>().text = text;
                }
                yield return new WaitForSeconds(float.Parse(tokens[3] == "" ? "0" : tokens[3]));

            }
            else if (fun == "question")
            {
                dialog.enabled = true;
                //front.enabled = false;
                //video.enabled = false;
                string ch = tokens[1];
                string text = question;

                dialog.transform.Find("Question_Image").GetComponent<Image>().enabled = true;
                dialog.transform.Find("Question_Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ch);
                dialog.transform.Find("Text_Center").GetComponent<TextMeshProUGUI>().text = text;

                yield return new WaitForSeconds(float.Parse(tokens[3] == "" ? "0" : tokens[3]));
            }
            else if (fun == "camera")
            {
                for (int i = 0; i < cameras.Length; i++) cameras[i].SetActive(false);
                cameras[int.Parse(tokens[1])].SetActive(true);
            }
            else if (fun == "show")
            {
                bool show = tokens[1] == "true";
                yield return StartCoroutine(FadeCanvas(show, 0.5f)); // Fade In or Fade Out
            }
            else if (fun == "moveto")
            {
                GameObject obj = map[tokens[1]];
                Vector3 target = map[tokens[2]].transform.position;
                float duration = float.Parse(tokens[3] == "" ? "3" : tokens[3]);

                float elapsedTime = 0f;
                Vector3 startingPos = obj.transform.position;
                while (elapsedTime < duration)
                {
                    obj.transform.position = Vector3.Lerp(startingPos, target, elapsedTime / duration);
                    obj.transform.forward = (target - obj.transform.position).normalized;
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                obj.transform.position = target;
            }
        }
        yield return StartCoroutine(FadeCanvas(false, 0.5f)); // Fade Out
    }

    private IEnumerator FadeCanvas(bool fadeIn, float duration)
    {
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    string[] Parsing(string cmd)
    {
        cmd = cmd.Replace("(", ",").Replace(")", "");
        string[] tokens = cmd.Split(',');
        for (int i = 0; i < tokens.Length; i++)
        {
            tokens[i] = tokens[i].Trim();
        }
        return tokens;
    }
}

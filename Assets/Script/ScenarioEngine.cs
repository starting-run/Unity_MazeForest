using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Collections.Generic;

public class ScenarioEngine : MonoBehaviour
{
    public Canvas canvas;
    public Canvas worldCanvas;
    public GameObject[] cameras;
    public string[] objectsName;

    private Image front;
    private RawImage video;
    private TextMeshProUGUI text;
    private Canvas dialog;
    private VideoPlayer player;
    private Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();
    private CanvasGroup canvasGroup;

    private Coroutine currentCoroutine;

    void Start()
    {
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        }

        //front = canvas.transform.Find("Front").GetComponent<Image>();
        video = worldCanvas.transform.Find("Video").GetComponent<RawImage>();
        //text = canvas.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        player = transform.GetComponent<VideoPlayer>();
        dialog = canvas.transform.Find("Dialog").GetComponent<Canvas>();
        if (objectsName.Length > 0)
        {
            foreach (string name in objectsName)
                map.Add(name, GameObject.Find(name));
        }
    }

    public IEnumerator PlayScript(string script, string question = "")
    {
        yield return StartCoroutine(FadeCanvas(true, 0f)); // Fade In
        foreach (string token in script.Split('\n'))
        {
            string[] tokens = Parsing(token);
            string fun = tokens[0];

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            if (fun == "video")
            {
                currentCoroutine = StartCoroutine(PlayVideo(tokens));
                yield return currentCoroutine;
            }
            else if (fun == "text")
            {
                currentCoroutine = StartCoroutine(DisplayText(tokens));
                yield return currentCoroutine;
            }
            else if (fun == "image")
            {
                currentCoroutine = StartCoroutine(DisplayImage(tokens));
                yield return currentCoroutine;
            }
            else if (fun == "wait")
            {
                currentCoroutine = StartCoroutine(Wait(tokens));
                yield return currentCoroutine;
            }
            else if (fun == "dialog")
            {
                currentCoroutine = StartCoroutine(DisplayDialog(tokens));
                yield return currentCoroutine;
            }
            else if (fun == "question")
            {
                currentCoroutine = StartCoroutine(DisplayQuestion(tokens, question));
                yield return currentCoroutine;
            }
            else if (fun == "camera")
            {
                currentCoroutine = StartCoroutine(SwitchCamera(tokens));
                yield return currentCoroutine;
            }
            else if (fun == "show")
            {
                currentCoroutine = StartCoroutine(FadeCanvas(bool.Parse(tokens[1]), 0.5f));
                yield return currentCoroutine;
            }
            else if (fun == "moveto")
            {
                currentCoroutine = StartCoroutine(MoveTo(tokens));
                yield return currentCoroutine;
            }
        }
        yield return StartCoroutine(FadeCanvas(false, 0.5f)); // Fade Out
    }

    private IEnumerator PlayVideo(string[] tokens)
    {
        string clip = tokens[1];
        //front.enabled = false;
        dialog.enabled = false;
        video.enabled = true;
        player.clip = Resources.Load<VideoClip>(clip);
        float len = (float)player.clip.length;
        player.Play();
        yield return new WaitForSeconds(len);
    }

    private IEnumerator DisplayText(string[] tokens)
    {
        text.enabled = true;
        text.text = tokens[1];
        yield return new WaitForSeconds(float.Parse(tokens[2] == "" ? "0" : tokens[2]));
    }

    private IEnumerator DisplayImage(string[] tokens)
    {
        //front.enabled = true;
        //video.enabled = false;
        dialog.enabled = false;
        //front.sprite = Resources.Load<Sprite>(tokens[1]);
        yield return new WaitForSeconds(float.Parse(tokens[2] == "" ? "0" : tokens[2]));
    }

    private IEnumerator Wait(string[] tokens)
    {
        yield return new WaitForSeconds(float.Parse(tokens[1] == "" ? "2" : tokens[1]));
    }

    private IEnumerator DisplayDialog(string[] tokens)
    {
        dialog.enabled = true;
        //front.enabled = false;
        //video.enabled = false;
        dialog.transform.Find("Question_Image").GetComponent<Image>().enabled = false;
        string ch = tokens[1];
        string text = tokens[2];
        if (ch != "null")
        {
            dialog.transform.Find("Image").GetComponent<Image>().enabled = true;
            dialog.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ch);
            dialog.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
            dialog.transform.Find("Text_Center").GetComponent<TextMeshProUGUI>().text = "";
            dialog.transform.Find("NpcName").GetComponent<Image>().enabled=true;
            dialog.transform.Find("npcNameT").GetComponent<TextMeshProUGUI>().text = ch;
        }
        else
        {
            dialog.transform.Find("Image").GetComponent<Image>().enabled = false;
            dialog.transform.Find("Text_Center").GetComponent<TextMeshProUGUI>().text = text;
            dialog.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            dialog.transform.Find("NpcName").GetComponent<Image>().enabled = false;
            dialog.transform.Find("npcNameT").GetComponent<TextMeshProUGUI>().text = "";
        }
        yield return new WaitForSeconds(float.Parse(tokens[3] == "" ? "0" : tokens[3]));
    }

    private IEnumerator DisplayQuestion(string[] tokens, string question)
    {
        dialog.enabled = true;
        string ch = tokens[1];
        dialog.transform.Find("Question_Image").GetComponent<Image>().enabled = true;
        dialog.transform.Find("Question_Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ch);
        dialog.transform.Find("Text_Center").GetComponent<TextMeshProUGUI>().text = question;

        dialog.transform.Find("Image").GetComponent<Image>().enabled = false;
        dialog.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";

        dialog.transform.Find("NpcName").GetComponent<Image>().enabled = false;
        dialog.transform.Find("npcNameT").GetComponent<TextMeshProUGUI>().text = "";
        yield return new WaitForSeconds(float.Parse(tokens[3] == "" ? "0" : tokens[3]));
    }

    private IEnumerator SwitchCamera(string[] tokens)
    {
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].SetActive(false);
        cameras[int.Parse(tokens[1])].SetActive(true);
        yield return null;
    }

    private IEnumerator MoveTo(string[] tokens)
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
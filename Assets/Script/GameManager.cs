using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private AudioSource audioSource;
    public AudioClip Success;
    public AudioClip Failure;

    public float playTime = 400;
    private float elapsedTime = 0;

    public bool isGameOver = false; // 게임 오버 상태
    public bool isGameClear = false; // 게임 오버 상태

    [SerializeField]
    private GameObject RemainingTime;
    [SerializeField]
    private TextMeshProUGUI timeText;
    // Start is called before the first frame update


    [Header("스크립트 엔진 설정")]
    public ScenarioEngine engine;
    private string script;

    [Header("엔딩에 표시할 요소")]
    public int FindNPC = 0;
    public int FailCount = 0; //문제를 실패한 횟수

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(DelayedStartCoroutine());
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    IEnumerator DelayedStartCoroutine()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(ShowInfoUI());
    }

    IEnumerator ShowInfoUI()
    {
        RemainingTime.SetActive(true);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameClear)
        {
            elapsedTime += Time.deltaTime; //경과시간

            float remainingTime = GameManager.Instance.playTime - elapsedTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            timeText.text = $"{minutes:D2}:{seconds:D2}";

            if (remainingTime <= 0)
            {
                isGameOver = true;
            }
        }

    }

    public void GameClear()
    {
        isGameClear = true;
        RemainingTime.SetActive(false);
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("게임 오버");
    }


    public void Target_Success()
    {
        PlaySound(Success);
        script = Resources.Load<TextAsset>("Success").ToString();
        StartCoroutine(engine.PlayScript(script));
    }

    public void Target_Fail()
    {
        PlaySound(Failure);
        script = Resources.Load<TextAsset>("Fail").ToString();
        StartCoroutine(engine.PlayScript(script));
        FailCount++;
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

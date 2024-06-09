using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private AudioSource audioSource;
    public AudioClip Success;
    public AudioClip Failure;

    [Header("게임 진행 시간")]
    public float playTime = 400;
    private float elapsedTime = 0;

    [Header("게임 진행 상황 확인")]
    public bool isGameOver = false; // 게임 오버 상태
    public bool isGameClear = false; // 게임 오버 상태
    public Material defaultSkybox;
    public Material successSkybox;
    public GameObject fireWorks;

    [Header("팝업 UI")]
    [SerializeField]
    private GameObject RemainingTime;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private GameObject Popup;
    private GameObject SuccessM;
    private GameObject Fail;
    [SerializeField]
    private GameObject restartButton2; //좌상단 재시작버튼
    private TextMeshProUGUI npcText;
    private TextMeshProUGUI failQuestionText;

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

        //팝업UI
        SuccessM = Popup.transform.Find("SuccessM").gameObject;
        Fail = Popup.transform.Find("Fail").gameObject;
        npcText = SuccessM.transform.Find("npc").GetComponent<TextMeshProUGUI>();
        failQuestionText = SuccessM.transform.Find("failQuestion").GetComponent<TextMeshProUGUI>();
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

    void Update()
    {
        if (!isGameClear && !isGameOver)
        {
            elapsedTime += Time.deltaTime; //경과시간

            float remainingTime = GameManager.Instance.playTime - elapsedTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            timeText.text = $"{minutes:D2}:{seconds:D2}";

            if (remainingTime <= 1)
            {
                isGameOver = true;
                GameOver();
            }

            // 시연 디버그용. 실패 했을 때를 보여주기 위함 (게임시간 15초 남김)
            if (Input.GetKeyDown(KeyCode.N))
            {
                elapsedTime += 285;
            }
        }
    }

    public void GameClear()
    {
        isGameClear = true;
        RemainingTime.SetActive(false);

        npcText.text = "총" + FindNPC + "명의 NPC를 찾았어요!";

        if (FailCount == 0)
        {
            failQuestionText.text = "문제를 모두 한번에 맞췄어요.\n정말 훌륭해요!";
        }
        else
        {
            failQuestionText.text = "문제를 " + FailCount + "번 틀렸네요.\n다음에는 좀 더 노력해봐요!";
        }


        SuccessM.SetActive(true);
        Fail.SetActive(false);
        Popup.SetActive(true);
        RenderSettings.skybox = successSkybox;
        fireWorks.SetActive(true);
    }

    private void GameOver()
    {
        isGameOver = true;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        /* 팝업 관련
         * 실패 했을 때엔 팝업을 닫지 않고 재시작 버튼만 활성화
         */
        SuccessM.SetActive(false);
        Fail.SetActive(true);
        Popup.SetActive(true);

        // 게임 실패시 시나리오 엔진 끄기
        engine.canvas.enabled = false;
        
        // 불꽃놀이는 당연히 끄기
        fireWorks.SetActive(false);
        Debug.Log("게임 오버");

    }

    public void ClosePopup()
    {
       Popup.SetActive(false);
       restartButton2.SetActive(true);
    }
    public void RestartScene()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
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

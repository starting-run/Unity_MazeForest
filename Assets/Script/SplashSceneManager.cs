using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasA;
    [SerializeField]
    private GameObject canvasB;
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private float delayBetweenCanvases = 4.0f;

    private AsyncOperation asyncLoad;

    private void Start()
    {
        canvasA.SetActive(true);
        canvasB.SetActive(false);

        Cursor.visible = false; //스플래시 마우스 비활성화
        StartCoroutine(LoadSceneAsync()); // 비동기 씬 로드를 시작합니다.

        // 캔버스 전환을 시작합니다.
        StartCoroutine(SwitchCanvasAndLoadScene());
    }

    private IEnumerator LoadSceneAsync()
    {
        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false; 
        yield return asyncLoad;
    }

    private IEnumerator SwitchCanvasAndLoadScene()
    {
        yield return new WaitForSeconds(delayBetweenCanvases);

        canvasA.SetActive(false);
        canvasB.SetActive(true);

        yield return new WaitForSeconds(delayBetweenCanvases);

        Cursor.visible = true; //스플래시 끝난 후 마우스 활성화
        asyncLoad.allowSceneActivation = true;
    }
}

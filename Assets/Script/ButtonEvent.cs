using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public void GameStart()
    {
        LoadingSceneControler.LoadScene("JumpMap");
    }

    public void GameExit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
                        Application.Quit();
    #endif
    }
}

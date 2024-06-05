using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneManager : MonoBehaviour
{
    public float SkipTime = 5.0f;

    void VideoEnd()
    {
        SceneManager.LoadScene("TitleScene");
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Invoke("VideoEnd", SkipTime);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

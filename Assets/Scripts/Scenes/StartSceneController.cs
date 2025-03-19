using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startButtonText;
    private AsyncOperation _asyncOp;
    private bool allowLoadNextScene = false;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioConfig.BGM4, true);
        startButtonText.text = "LOADING...";
        StartCoroutine(LoadNextSceneAsync());
    }

    public void OnStartButtonClick()
    {
        if (!allowLoadNextScene) return;
        AudioManager.Instance.PlaySound2D(AudioConfig.BUTTON_CLICK);
        _asyncOp.allowSceneActivation = true;
    }

    private IEnumerator LoadNextSceneAsync()
    {
        _asyncOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        _asyncOp.allowSceneActivation = false;
        while(true)
        {
            float progress = Mathf.Clamp01(_asyncOp.progress / 0.9f);
            if(progress >= 1)
            {
                startButtonText.text = "CLICK TO START!";
                allowLoadNextScene = true;
                break;
            }
            yield return null;
        }
    }
}

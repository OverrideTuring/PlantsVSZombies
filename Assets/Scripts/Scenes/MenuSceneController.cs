using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuSceneController : MonoBehaviour
{
    [SerializeField] private GameObject changeNameDialog;
    [SerializeField] private GameObject optionsDialog;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup blockingPanel;

    public static MenuSceneController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        UpdateUsernameText();
    }

    private void Start()
    {
        if (!AudioManager.Instance.IsPlaying())
        {
            AudioManager.Instance.PlayMusic(AudioConfig.BGM4, true);
        }
    }

    public void UpdateUsernameText()
    {
        usernameText.text = PlayerPrefs.GetString("username", "-");
    }

    public void OnChangeNameButtonClick()
    {
        OnDialogOpen();
        changeNameDialog.SetActive(true);
        AudioManager.Instance.PlaySound2D(AudioConfig.TAP);
    }

    public void OnOptionsButtonClick()
    {
        OnDialogOpen();
        optionsDialog.SetActive(true);
        AudioManager.Instance.PlaySound2D(AudioConfig.TAP);
    }

    public void OnHelpButtonClick()
    {
        // TODO: 帮助界面UI
        // OnDialogOpen();
        // helpDialog.SetActive(true);
        AudioManager.Instance.PlaySound2D(AudioConfig.TAP);
    }

    public void OnQuitButtonClick()
    {
        // OnDialogOpen();
        // confirmDialog.SetActive(true);
        AudioManager.Instance.PlaySound2D(AudioConfig.TAP);
        ExitGame();
    }

    public async void OnAdventureButtonClick()
    {
        blockingPanel.blocksRaycasts = true;

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(SceneConfig.GAME_SCENE);
        asyncOp.allowSceneActivation = false;

        AudioManager.Instance.PlayMusic(AudioConfig.LOSE_MUSIC, false);
        await Task.Delay(1500);
        float duration = AudioManager.Instance.PlaySound2D(AudioConfig.EVIL_LAUGH);
        await Task.Delay((int)(duration * 1000));

        asyncOp.allowSceneActivation = true;
    }

    public void OnDialogOpen()
    {
        blockingPanel.blocksRaycasts = true;
    }

    public void OnDialogClose()
    {
        blockingPanel.blocksRaycasts = false;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

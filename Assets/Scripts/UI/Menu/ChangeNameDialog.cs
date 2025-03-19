using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeNameDialog : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInputField;

    private void OnEnable()
    {
        nameInputField.text = PlayerPrefs.GetString("username", "");
    }

    private void OnDisable()
    {
        MenuSceneController.Instance.OnDialogClose();
    }

    public void OnRenameButtonClick()
    {
        if (nameInputField.text == "") return;
        PlayerPrefs.SetString("username", nameInputField.text);
        MenuSceneController.Instance.UpdateUsernameText();
        AudioManager.Instance.PlaySound2D(AudioConfig.BUTTON_CLICK);
    }

    public void OnDeleteButtonClick()
    {
        nameInputField.text = "";
        AudioManager.Instance.PlaySound2D(AudioConfig.BUTTON_CLICK);
    }

    public void OnOKButtonClick()
    {
        OnRenameButtonClick();
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySound2D(AudioConfig.BUTTON_CLICK);
    }

    public void OnCancelButtonClick()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySound2D(AudioConfig.BUTTON_CLICK);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : BaseButton
{
    [SerializeField] private GameObject levelMenuDialog;

    private void Start()
    {
        gameObject.SetActive(false);
        GameEventManager.Instance.OnLevelStart += OnLevelStart;
    }

    public void OnLevelStart()
    {
        gameObject.SetActive(true);
    }

    public override void OnClick() {
        levelMenuDialog.SetActive(true);
        AudioManager.Instance.PlaySound2D(AudioConfig.PAUSE);
        PauseManager.Instance.Pause();
    }
}

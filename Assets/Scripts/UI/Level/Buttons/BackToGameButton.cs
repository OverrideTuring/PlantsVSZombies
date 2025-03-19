using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackToGameButton : BaseButton
{
    [SerializeField] private GameObject levelMenuDialog;
    [SerializeField] private Sprite normalImage;
    [SerializeField] private Sprite pressedImage;
    private Vector3 textOriginalPosition;

    protected override void Awake()
    {
        base.Awake();
        textOriginalPosition = textMeshProUGUI.transform.localPosition;
    }

    public override void OnPointerDown()
    {
        AudioManager.Instance.PlaySound2D(AudioConfig.GRAVE_BUTTON);
        buttonImage.sprite = pressedImage;
        Vector3 newPosition = textOriginalPosition;
        newPosition.y -= 2;
        textMeshProUGUI.transform.localPosition = newPosition;
    }

    public override void OnPointerUp()
    {
        buttonImage.sprite = normalImage;
        textMeshProUGUI.transform.localPosition = textOriginalPosition;
    }

    public override void OnClick() {
        levelMenuDialog.SetActive(false);
        AudioManager.Instance.PlaySound2D(AudioConfig.TAP);
        PauseManager.Instance.Resume();
    }
}

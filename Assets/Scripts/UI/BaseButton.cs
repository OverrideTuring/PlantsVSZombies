using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    [SerializeField] protected Image buttonImage;
    protected Color originalColor;
    protected Vector3 originalPosition;

    protected virtual void Awake()
    {
        originalColor = textMeshProUGUI.color;
        originalPosition = buttonImage.transform.localPosition;
    }

    public virtual void OnPointerEnter()
    {
        textMeshProUGUI.color = originalColor * 1.5f;
    }

    public virtual void OnPointerExit()
    {
        textMeshProUGUI.color = originalColor;
    }

    public virtual void OnPointerDown()
    {
        AudioManager.Instance.PlaySound2D(AudioConfig.GRAVE_BUTTON);
        Vector3 newPosition = buttonImage.transform.localPosition;
        newPosition.x += 2;
        newPosition.y -= 2;
        buttonImage.transform.localPosition = newPosition;
    }

    public virtual void OnPointerUp()
    {
        buttonImage.transform.localPosition = originalPosition;
    }

    public virtual void OnClick() {}
}

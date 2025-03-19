using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private CanvasGroup blockingPanel;
    [SerializeField] private GameObject menuDialog;

    private void Awake()
    {
        Instance = this;
    }

    public void PutOnBlockingPanel()
    {
        blockingPanel.blocksRaycasts = true;
    }

    public void PutOffBlockingPanel()
    {
        blockingPanel.blocksRaycasts = false;
    }

    public void ShowMenuDialog()
    {
        menuDialog.SetActive(true);
    }

    public void CancelMenuDialog()
    {
        menuDialog.SetActive(false);
    }
}

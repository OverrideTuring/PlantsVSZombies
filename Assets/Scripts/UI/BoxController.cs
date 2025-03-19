using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] private GameObject leftBlackBox;
    [SerializeField] private GameObject rightBlackBox;

    private void Awake()
    {
        ChangeBoxSize(GameConfig.FullScreen);
    }

    private void OnEnable()
    {
        GameConfig.OnFullScreenChanged += ChangeBoxSize;
    }

    private void OnDisable()
    {
        GameConfig.OnFullScreenChanged -= ChangeBoxSize;
    }

    private void ChangeBoxSize(bool fullScreen)
    {
        int screenWidth, screenHeight;
        if (fullScreen)
        {
            screenWidth = GameConfig.fullScreenRes.x;
            screenHeight = GameConfig.fullScreenRes.y;
        }
        else
        {
            screenWidth = GameConfig.windownScreenRes.x;
            screenHeight = GameConfig.windownScreenRes.y;
        }
        int height = screenHeight;
        int width = screenWidth / 2 - 2 * screenHeight / 3;
        leftBlackBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        leftBlackBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rightBlackBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rightBlackBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}

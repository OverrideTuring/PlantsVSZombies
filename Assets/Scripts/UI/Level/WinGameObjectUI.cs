using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class WinGameObjectUI : MonoBehaviour
{
    [SerializeField] private WinObjectData data;
    [SerializeField] private float minDistance = 50, maxDistance = 150;
    [SerializeField] private Vector2 margin = new Vector2(200, 200);

    public WinObjectType Type { get { return data.winObjectType; } }
    public PlantType PlantType { get {  return data.plantType; } }

    public void SetData(WinObjectData winObjectData)
    {
        data = winObjectData;
        Image image = GetComponent<Image>();
        image.sprite = data.sourceImage;
        image.SetNativeSize();
        GetComponent<RectTransform>().localScale = Vector3.one * data.originalScale;
    }

    public void OnClick()
    {
        LevelManager.Instance.ChangeGameState(GameState.Normal);
        UIManager.Instance.PutOnBlockingPanel();
        GetComponent<Button>().enabled = false;
        AudioManager.Instance.PlayMusic(AudioConfig.WIN_MUSIC);

        RectTransform rectTransform = GetComponent<RectTransform>();
        Sequence winSequence = DOTween.Sequence();
        // 同时执行位置和缩放动画
        winSequence
            .Append(rectTransform.DOAnchorPos(new Vector2(0, 0), 3.0f).SetEase(Ease.OutQuad))
            .Join(rectTransform.DOScale(rectTransform.localScale * 1.5f, 3.0f).SetEase(Ease.OutQuad)) // 缩放至1.5倍
            .OnComplete(() => LevelManager.Instance.OnWinGameObjectPickedUp(this));
    }

    public void FlyAround()
    {
        float x = transform.position.x + (Random.value < 0.5 ? -1 : 1) * Random.Range(minDistance, maxDistance);
        float y = transform.position.y + (Random.value < 0.5 ? -1 : 1) * Random.Range(minDistance, maxDistance);
        float z = transform.position.z;
        x = Mathf.Clamp(x, margin[0], Screen.width - margin[0]);
        y = Mathf.Clamp(y, margin[1], Screen.height - margin[1]);
        Vector3 targetPosition = new Vector3(x, y, z);
        Vector3 middlePosition = transform.position + (targetPosition - transform.position) / 2.0f;
        float distance = (targetPosition - transform.position).magnitude;
        middlePosition.y += distance / 2.0f;
        transform.DOPath(new Vector3[] { transform.position, middlePosition, targetPosition },
            0.7f, PathType.CatmullRom).SetEase(Ease.OutQuad);
    }
}

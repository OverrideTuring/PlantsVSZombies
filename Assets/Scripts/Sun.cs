using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Sun : MonoBehaviour, IPausable, IPoolObject
{
    [Header("Settings")]
    public float jumpDuration = 1f;
    public float moveDuration = 0.5f;
    public float dropDuration = 5f;
    public float timeToLive = 10f;
    public int point = 25;
    private Animator anim;
    private float pauseAnimSpeed;
    private bool paused = false;

    public GameObject Prefab { get => PrefabConfig.Instance.SunPrefab; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        
        StartCoroutine(Disappear());
    }

    private void OnEnable()
    {
        PauseManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        PauseManager.Instance.Unregister(this);
    }

    public void JumpTo(Vector3 target)
    {
        Vector3 midPos = (transform.position + target) / 2;
        float height = Vector3.Distance(transform.position, target) / 2;
        midPos.y += height;
        transform.DOPath(new Vector3[] { transform.position, midPos, target },
            jumpDuration, PathType.CatmullRom).SetEase(Ease.OutQuad).SetId(this);
    }

    public void DropDown(Vector3 target)
    {
        transform.DOMove(target, dropDuration).SetId(this); ;
    }

    public void OnMouseDown()
    {
        if (paused) return;
        AudioManager.Instance.PlaySound(AudioConfig.POINTS, transform.position);
        transform.DOMove(SunManager.Instance.sunImagePosition, moveDuration).
            SetEase(Ease.OutQuad).OnComplete(
            () =>
            {
                // Destroy(gameObject);
                AddToPool();
                SunManager.Instance.UpdateSunPoint(point);
            }
            ).SetId(this);
    }

    private IEnumerator Disappear()
    {
        yield return new PausableWaitForSeconds(timeToLive);
        // Destroy(gameObject);
        AddToPool();
    }

    public void OnPause()
    {
        pauseAnimSpeed = anim.speed;
        anim.speed = 0;
        paused = true;
    }

    public void OnResume()
    {
        anim.speed = pauseAnimSpeed;
        paused = false;
    }

    public void ResetState()
    {
        StartCoroutine(Disappear());
    }

    public void AddToPool()
    {
        StopAllCoroutines();
        DOTween.Kill(this);
        PoolManager.Instance.AddGameObject(this);
    }

    private void OnDestroy()
    {
        PauseManager.Instance.Unregister(this);
    }
}

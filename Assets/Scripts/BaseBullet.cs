using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseBullet : MonoBehaviour, IPausable, IPoolObject
{
    [Header("Basic Settings")]
    public int damage = 20;
    bool paused = false;

    public virtual GameObject Prefab { get; }

    private void OnEnable()
    {
        PauseManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        PauseManager.Instance.Unregister(this);
    }

    private void Update()
    {
        if (paused) return;
        MoveUpdate();
    }

    protected virtual void MoveUpdate(){}

    public void OnPause()
    {
        paused = true;
    }

    public void OnResume()
    {
        paused = false;
    }

    public virtual void ResetState()
    {

    }

    public void AddToPool()
    {
        StopAllCoroutines();
        PoolManager.Instance.AddGameObject(this);
    }

    protected void OnDestroy()
    {
        PauseManager.Instance.Unregister(this);
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlantState
{
    None,
    Disable,
    Enable
}

public class Plant : MonoBehaviour, IPausable, IPoolObject
{
    [Header("Basic Settings")]
    public PlantState plantState = PlantState.Disable;
    public PlantType plantType = PlantType.Sunflower;
    public int fullHP = 300;
    [SerializeField] protected int currentHP;
    protected Animator anim;
    protected float pauseAnimSpeed;
    protected PlantState pausePlantState;
    public event Action OnPlantDie;

    public virtual GameObject Prefab { get; }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        currentHP = fullHP;
    }

    protected virtual void Start()
    {
        
    }

    private void OnEnable()
    {
        PauseManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        PauseManager.Instance.Unregister(this);
    }

    void Update()
    {
        switch (plantState)
        {
            case PlantState.Disable:
                DisableUpdate();
                break;
            case PlantState.Enable:
                EnableUpdate();
                break;
            default:
                break;
        }
    }

    private void DisableUpdate()
    {

    }

    protected virtual void EnableUpdate()
    {

    }

    public void TransitionToDisable()
    {
        plantState = PlantState.Disable;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public void TransitionToEnable()
    {
        plantState = PlantState.Enable;
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public virtual void ReceiveDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            AudioManager.Instance.PlaySound(AudioConfig.GULP, transform.position);
            Die();
        }
    }

    public void GotHitEffect()
    {
        anim.SetTrigger("gotHit");
    }

    protected void Die()
    {
        AddToPool();
        // Destroy(gameObject);
        OnPlantDie?.Invoke();
    }

    public void OnPause()
    {
        pausePlantState = plantState;
        pauseAnimSpeed = anim.speed;
        plantState = PlantState.None;
        anim.speed = 0;
    }

    public void OnResume()
    {
        plantState = pausePlantState;
        anim.speed = pauseAnimSpeed;
    }

    public virtual void ResetState()
    {
        currentHP = fullHP;
        anim.Play("New State");
        TransitionToDisable();
    }

    public virtual void AddToPool()
    {
        StopAllCoroutines();
        PoolManager.Instance.AddGameObject(this);
    }

    private void OnDestroy()
    {
        PauseManager.Instance.Unregister(this);
    }
}

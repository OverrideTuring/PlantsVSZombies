using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallNut : Plant
{
    private Tween delayTween;
    [SerializeField] private Sprite wallNutHealthy;

    public override GameObject Prefab => PrefabConfig.Instance.WallNutPrefab;

    protected override void Awake()
    {
        base.Awake();
        wallNutHealthy = GetComponent<SpriteRenderer>().sprite;
    }

    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);
        if (currentHP > 0) 
        {
            anim.SetFloat("speed", 0);
            anim.SetInteger("health", currentHP);
            if(delayTween != null && delayTween.IsActive())
            {
                delayTween.Kill();
            }

            delayTween = DOVirtual.DelayedCall(0.3f, () =>
            {
                anim.SetFloat("speed", 1);
            });
            delayTween.Play();
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        anim.SetInteger("health", fullHP);
        GetComponent<SpriteRenderer>().sprite = wallNutHealthy;
    }
}

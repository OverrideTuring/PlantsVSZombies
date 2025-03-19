using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : BaseBullet
{
    [Header("Specific Settings")]
    public float speed = 5f;
    [SerializeField] private GameObject peaBulletHitPrefab;

    public override GameObject Prefab => PrefabConfig.Instance.PeaBulletPrefab;

    private void Start()
    {
        StartCoroutine(Disappear());
    }

    protected override void MoveUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Zombie"))
        {
            Destroy(gameObject);
            AudioManager.Instance.PlaySound(AudioConfig.GetRandomSplat(), transform.position);
            collision.gameObject.GetComponentInParent<Zombie>().ReceiveDamage(damage);
            GameObject peaBulletHit = Instantiate(peaBulletHitPrefab, transform.position, Quaternion.identity);
            Destroy(peaBulletHit, 1.0f);
        }
    }

    private IEnumerator Disappear()
    {
        yield return new PausableWaitForSeconds(10.0f);
        // Destroy(gameObject);
        AddToPool();
    }

    public override void ResetState()
    {
        base.ResetState();
        StartCoroutine(Disappear());
    }
}

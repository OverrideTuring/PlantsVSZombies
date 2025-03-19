using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{
    [Header("Settings")]
    public float shootInterval = 1.4f;
    public Transform bulletStartPoint;
    public PeaBullet peaBulletPrefab;
    private Coroutine shootCoroutine;
    private int detectedZombieCount = 0;

    public override GameObject Prefab => PrefabConfig.Instance.PeashooterPrefab;

    private void OnDisable()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
    }

    private IEnumerator shootWithInterval()
    {
        while (true)
        {
            float timer = -Time.deltaTime;
            while(true)
            {
                if (plantState != PlantState.Enable)
                {
                    yield return null;
                    continue;
                }
                timer += Time.deltaTime;
                if(timer >= shootInterval) break;
                yield return null;
            }
            shoot();
        }
    }
    public void shoot()
    {
        AudioManager.Instance.PlaySound(AudioConfig.THROW, transform.position);
        Instantiate(peaBulletPrefab, bulletStartPoint.position, Quaternion.identity);
    }

    public void OnDetectedZombieIncrease()
    {
        if (detectedZombieCount == 0)
        {
            shootCoroutine = StartCoroutine(shootWithInterval());
        }
        detectedZombieCount++;
    }

    public void OnDetectedZombieDecrease()
    {
        detectedZombieCount--;
        if (detectedZombieCount == 0)
        {
            StopCoroutine(shootCoroutine);
        }
    }
}

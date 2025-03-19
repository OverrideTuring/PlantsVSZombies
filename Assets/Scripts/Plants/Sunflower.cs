using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plant
{
    [Header("Settings")]
    public GameObject sunPrefab;
    public float firstTimeInterval = 4;
    public float spawnTimeInterval = 25;
    public float minSunDistance = 1;
    public float maxSunDistance = 3;
    private bool afterFirst = false;
    private float currentTime = 0;
    [SerializeField]
    private float minX = -4.0f;
    private float maxX = 4.0f;
    private bool canSpawn = true;

    public override GameObject Prefab => PrefabConfig.Instance.SunflowerPrefab;

    protected override void Start()
    {
        base.Start();
        GameEventManager.Instance.OnWinGame += OnWinGame;
    }

    protected override void EnableUpdate()
    {
        base.EnableUpdate();
        if (!canSpawn) return;
        currentTime += Time.deltaTime;
        if (!afterFirst)
        {
            if(currentTime >= firstTimeInterval)
            {
                anim.SetTrigger("spawnSunPoint");
                currentTime = 0;
                afterFirst = true;
            }
        }
        else if(currentTime >= spawnTimeInterval)
        {
            anim.SetTrigger("spawnSunPoint");
            currentTime = 0;
        }
    }

    public void SpawnSunPoint()
    {
        // GameObject sun = Instantiate(sunPrefab, transform.position, Quaternion.identity);
        GameObject sun = PoolManager.Instance.GetGameObject(sunPrefab);
        sun.transform.position = transform.position;

        float distance = (Random.value < 0.5 ? -1 : 1) * Random.Range(minSunDistance, maxSunDistance);
        Vector3 position = transform.position;
        position.x += distance;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z -= 0.1f;
        sun.GetComponent<Sun>().JumpTo(position);
    }

    private void OnWinGame(Zombie zombie)
    {
        canSpawn = false;
        GameEventManager.Instance.OnWinGame -= OnWinGame;
    }

    public override void ResetState()
    {
        base.ResetState();
        afterFirst = false;
    }
}

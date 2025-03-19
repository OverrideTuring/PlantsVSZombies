using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class SunSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject sunPrefab;
    public float minSpawnInterval = 6f;
    public float maxSpawnInterval = 11f;
    public float spawnRandomness = 2f;
    public float minDropY = 3;
    public float maxDropY = 10;
    private float currentSpawnInterval;
    private Vector2 spawnArea;
    Coroutine spawnCoroutine;

    private void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        spawnArea.x = collider.size.x;
        spawnArea.y = collider.size.y;
        gameObject.SetActive(false);
        currentSpawnInterval = minSpawnInterval;
    }

    private void Start()
    {
        GameEventManager.Instance.OnWinGame += OnWinGame;
    }

    private void OnEnable()
    {
        spawnCoroutine = StartCoroutine(SpawnSunWithInterval());
    }

    private void OnDisable()
    {
        if(spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    private IEnumerator SpawnSunWithInterval()
    {
        while (true)
        {
            float nextSpawnInterval = currentSpawnInterval + Random.Range(-spawnRandomness, spawnRandomness);
            currentSpawnInterval = Mathf.Clamp(currentSpawnInterval + 0.1f, minSpawnInterval, maxSpawnInterval);
            yield return new PausableWaitForSeconds(nextSpawnInterval);
            SpawnSun();
        }
    }

    private void SpawnSun()
    {
        float x = transform.position.x + Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float y = transform.position.y;
        // Spawn Sun
        Vector3 spawnPosition = new Vector3(x, y, -0.1f);
        // GameObject sun = Instantiate(sunPrefab, spawnPosition, Quaternion.identity);
        GameObject sun = PoolManager.Instance.GetGameObject(sunPrefab);
        sun.transform.position = spawnPosition;
        // Drop Sun
        Vector3 dropPosition = spawnPosition;
        dropPosition.y -= Random.Range(minDropY, maxDropY);
        sun.GetComponent<Sun>().DropDown(dropPosition);
    }
    private void OnWinGame(Zombie zombie)
    {
        StopCoroutine(spawnCoroutine);
        GameEventManager.Instance.OnWinGame -= OnWinGame;
    }
}

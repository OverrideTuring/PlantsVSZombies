using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }
    [Header("Settings")]
    public List<Transform> spawnPoints;
    private Queue<ZombieWaveData> zombieWaveDataQueue = new Queue<ZombieWaveData>();
    private List<int> waveZombieCountList = new List<int>();
    private Coroutine spawnZombieCoroutine;
    private Coroutine spawnGroanCoroutine;
    private bool firstZombieSpawned = false;
    private bool lastZombieSpawned = false;
    private int zombieCount = 0;
    private float groanProb = 1.0f;
    private int groanCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevelData();
        GameEventManager.Instance.OnZombieDie += OnZombieDecrease;
    }

    private void LoadLevelData()
    {
        foreach(var zombieWaveData in LevelManager.Instance.LevelData.zombieWaveDataList)
        {
            zombieWaveDataQueue.Enqueue(zombieWaveData);
            waveZombieCountList.Add(zombieWaveData.zombieSpawnDataList.Count);
        }
    }

    public List<int> getWaveZombieCountList()
    {
        return waveZombieCountList;
    }

    public bool ReachLastWave()
    {
        return zombieWaveDataQueue.Count == 0;
    }

    public void StartSpawnZombies()
    {
        if(zombieWaveDataQueue.Count > 0)
        {
            lastZombieSpawned = false;
            ZombieWaveData zombieWaveData = zombieWaveDataQueue.Dequeue();
            spawnZombieCoroutine = StartCoroutine(SpawnZombies(zombieWaveData));
            if(spawnGroanCoroutine == null)
            {
                spawnGroanCoroutine = StartCoroutine(SpawnGroanSound());
            }
        }
    }

    public void EndSpawnZombies()
    {
        StopCoroutine(spawnZombieCoroutine);
        StopCoroutine(spawnGroanCoroutine);
    }

    private IEnumerator SpawnZombies(ZombieWaveData zombieWaveData)
    {
        foreach(ZombieSpawnData zombieSpawnData in zombieWaveData.zombieSpawnDataList)
        {
            yield return new PausableWaitForSeconds(zombieSpawnData.spawnInterval);
            if (!firstZombieSpawned)
            {
                AudioManager.Instance.PlaySound2D(AudioConfig.AWOOGA);
                firstZombieSpawned = true;
            }
            Zombie zombie = SpawnZombie(zombieSpawnData.zombiePrefab);
            GameEventManager.Instance.TriggerZombieSpawned(zombie);
        }
        lastZombieSpawned = true;
    }

    public Zombie SpawnZombie(Zombie zombiePrefab)
    {
        int row = Random.Range(0, spawnPoints.Count);
        // Zombie zombie = Instantiate(zombiePrefab, spawnPoints[row].position, Quaternion.identity);
        Zombie zombie = PoolManager.Instance.GetGameObject(zombiePrefab.gameObject).GetComponent<Zombie>();
        zombie.transform.position = spawnPoints[row].position;
        zombie.GetComponent<SpriteRenderer>().sortingOrder = row + 1;
        zombieCount++;
        return zombie;
    }

    private IEnumerator SpawnGroanSound()
    {
        while (true)
        {
            if (zombieCount == 0)
            {
                yield return new PausableWaitForSeconds(1.0f);
                continue;
            }
            if(Random.value <= groanProb)
            {
                // 第一次呻吟声要在Awooga音乐之后
                if(groanCount == 0)
                {
                    yield return new PausableWaitForSeconds(4.0f);
                }
                AudioManager.Instance.PlaySound2D(AudioConfig.GetRandomGroan());
                groanCount++;
                if(groanCount == 8)
                {
                    groanProb = 0.2f;
                }
            }
            yield return new PausableWaitForSeconds(10.0f);
        }
    }

    public void OnZombieDecrease(Zombie zombie)
    {
        zombieCount--;
        if(lastZombieSpawned && zombieCount == 0)
        {
            if (zombieWaveDataQueue.Count == 0)
            {
                GameEventManager.Instance.TriggerWinGame(zombie);
            }
            else
            {
                GameEventManager.Instance.TriggerWaveEnd();
            }
        }
    }
}

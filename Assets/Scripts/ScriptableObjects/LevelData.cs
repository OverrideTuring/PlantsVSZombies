using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ZombieWaveData
{
    public List<ZombieSpawnData> zombieSpawnDataList;
}

[System.Serializable]
public struct ZombieSpawnData
{
    public Zombie zombiePrefab;
    public float spawnInterval;
}

[CreateAssetMenu(fileName ="LevelData", menuName = "ScriptableObject/Level")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int originalSunPoints;
    public List<ZombieWaveData> zombieWaveDataList;
    public WinObjectData winObjectData;
}

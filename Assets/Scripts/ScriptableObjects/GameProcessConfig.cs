using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameProcessConfig", menuName = "ScriptableObject/Game Process Config")]
public class GameProcessConfig : ScriptableObject
{
    public List<LevelData> levelDataList;

    public LevelData GetLevelData(int index)
    {
        if (levelDataList == null || levelDataList.Count <= index) return null;
        LevelData levelData = levelDataList[index];
        return levelData;
    }
}

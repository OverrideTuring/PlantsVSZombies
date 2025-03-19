using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantType
{
    Sunflower,
    Peashooter,
    CherryBomb,
    WallNut
}

[System.Serializable]
public class PlantData
{
    public PlantType plantType;    // 植物类型
    public Card cardPrefab;        // 卡牌预制体
    public Plant plantPrefab;      // 植物实体预制体
    public string plantName;       // 植物名字
    public string description;     // 植物描述
}

[CreateAssetMenu(fileName = "PlantConfig", menuName = "ScriptableObject/Plant Config")]
public class PlantConfig : ScriptableObject
{
    public List<PlantData> plantDataList = new List<PlantData>();

    private Dictionary<PlantType, PlantData> plantDataDict;

    public void Initialize()
    {
        plantDataDict = new Dictionary<PlantType, PlantData>();
        foreach (var data in plantDataList)
        {
            plantDataDict[data.plantType] = data;
        }
    }

    public PlantData GetPlantData(PlantType type)
    {
        if (plantDataDict == null) Initialize();
        return plantDataDict.TryGetValue(type, out var data) ? data : null;
    }

    public Plant GetPlantPrefab(PlantType type)
    {
        PlantData plantData = GetPlantData(type);
        return plantData != null ? plantData.plantPrefab : null;
    }
    public Card GetCardPrefab(PlantType type)
    {
        PlantData plantData = GetPlantData(type);
        return plantData != null ? plantData.cardPrefab : null;
    }
}

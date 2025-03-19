using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFactory : MonoBehaviour
{
    [SerializeField] PlantConfig plantConfig;

    public static PlantFactory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        plantConfig.Initialize();
    }

    public PlantData GetPlantData(PlantType plantType) { 
        PlantData plantData = plantConfig.GetPlantData(plantType);
        if(plantData == null)
        {
            Debug.LogError($"未找到 {plantType} 对应的植物数据");
            return null;
        }
        return plantData;
    }

    public Plant GetPlantPrefab(PlantType plantType)
    {
        Plant plantPrefab = plantConfig.GetPlantPrefab(plantType);
        if (plantPrefab == null)
        {
            Debug.LogError($"未找到 {plantType} 对应的卡牌预制体");
            return null;
        }
        return plantPrefab;
    }

    public Card GetCardPrefab(PlantType plantType)
    {
        Card cardPrefab = plantConfig.GetCardPrefab(plantType);
        if (cardPrefab == null)
        {
            Debug.LogError($"未找到 {plantType} 对应的卡牌预制体");
            return null;
        }
        return plantConfig.GetCardPrefab(plantType);
    }

    public Plant CreatePlant(PlantType plantType)
    {
        Plant plantPrefab = plantConfig.GetPlantPrefab(plantType);
        if (plantPrefab == null)
        {
            Debug.LogError($"未找到 {plantType} 对应的卡牌预制体");
            return null;
        }
        // Plant plant = Instantiate(plantPrefab);
        Plant plant = PoolManager.Instance.GetGameObject(plantPrefab.gameObject).GetComponent<Plant>();
        return plant;
    }

    public Card CreateCard(PlantType plantType)
    {
        Card cardPrefab = plantConfig.GetCardPrefab(plantType);
        if (cardPrefab == null)
        {
            Debug.LogError($"未找到 {plantType} 对应的卡牌预制体");
            return null;
        }
        Card card = Instantiate(cardPrefab);
        return card;
    }
}

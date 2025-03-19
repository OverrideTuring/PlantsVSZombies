using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private static List<PlantType> plants = new List<PlantType> { 
        PlantType.Sunflower,
        PlantType.Peashooter
    };

    public static List<PlantType> Plants {  get { return plants; } }
    public static int CurrentLevelIndex { get; set; } = 0;

    public static void AddPlant(PlantType plant)
    {
        plants.Add(plant);
    }
}

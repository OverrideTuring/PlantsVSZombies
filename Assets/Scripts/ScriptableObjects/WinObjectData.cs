using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WinObjectType
{
    PlantCard,
    Money,
    Shovel,
    ZombieNote
}

[CreateAssetMenu(fileName = "WinObjectData", menuName = "ScriptableObject/Win Object")]
public class WinObjectData : ScriptableObject
{
    public WinObjectType winObjectType;
    public PlantType plantType;
    public Sprite sourceImage;
    public float originalScale;
    public string description;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabConfig: MonoBehaviour
{
    public static PrefabConfig Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject sunPrefab;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject sunflowerPrefab;
    [SerializeField] private GameObject peashooterPrefab;
    [SerializeField] private GameObject wallNutPrefab;
    [SerializeField] private GameObject peaBulletPrefab;

    public GameObject SunPrefab { get => sunPrefab; }
    public GameObject ZombiePrefab { get => zombiePrefab; }
    public GameObject SunflowerPrefab { get => sunflowerPrefab; }
    public GameObject PeashooterPrefab { get => peashooterPrefab; }
    public GameObject WallNutPrefab { get => wallNutPrefab; }
    public GameObject PeaBulletPrefab { get => peaBulletPrefab; }
}

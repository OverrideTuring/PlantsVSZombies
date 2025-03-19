using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    public static SunManager Instance {  get; private set; }
    [SerializeField] private TextMeshProUGUI sunPointText;
    [SerializeField] private Transform sunImageTransform;
    public Vector3 sunImagePosition { get { return sunImageTransform.position; } }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private int sunPoints;

    public int SunPoints { get { return sunPoints; } }

    void Start()
    {
        sunPoints = LevelManager.Instance.LevelData.originalSunPoints;
        UpdateSunPointText();
    }

    private void UpdateSunPointText()
    {
        sunPointText.text = sunPoints.ToString();
    }

    public void UpdateSunPoint(int DeltaSunPoint)
    {
        sunPoints += DeltaSunPoint;
        UpdateSunPointText();
    }
}

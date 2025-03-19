using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private Card plantCard;
    private Plant plant;
    private Cell cell;

    public bool takePlant(Card plantCard)
    {
        if (plant != null) return false;
        plant = PlantFactory.Instance.CreatePlant(plantCard.plantType);
        if (plant == null) return false;
        plant.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        plant.TransitionToDisable();
        this.plantCard = plantCard;
        return true;
    }

    private void Update()
    {
        if (plant == null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelManager.Instance.ToggleGameState();
            }
            return;
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacePlant();
            return;
        }
        FollowCursor();
    }

    private void CancelPlacePlant()
    {
        plantCard.CancelPlacePlant();
        cell.CancelPlacePlant();
        plant.AddToPool();
        // Destroy(plant.gameObject);
        plant = null;
    }

    private void FollowCursor()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        plant.transform.position = mouseWorldPosition;
    }
    public void ReleasePlant()
    {
        if(plant == null) return;
        plant = null;
        plantCard.AfterPlacePlant();
    }

    public Plant GetPlant()
    {
        return plant;
    }

    public void SetCell(Cell cell)
    {
        this.cell = cell;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    private Plant plant;
    private Plant plantToPlace;
    private GameObject transparentImage;

    private void OnMouseEnter()
    {
        plantToPlace = HandManager.Instance.GetPlant();
        HandManager.Instance.SetCell(this);
        if(plantToPlace != null)
        {
            transparentImage = new GameObject(plantToPlace.plantType.ToString() + "_TransparentImage");
            transparentImage.transform.SetParent(transform);
            transparentImage.transform.localPosition = new Vector3(0, 0, 0);
            SpriteRenderer spriteRenderer = transparentImage.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = plantToPlace.GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.color = new Color(1, 1, 1, 0.5882f);
            spriteRenderer.sortingLayerName = "Game";
        }
    }

    private void OnMouseExit()
    {
       CancelPlacePlant();
    }

    private void OnMouseDown()
    {
        if (plant != null || plantToPlace == null)
        {
            return;
        }
        HandManager.Instance.ReleasePlant();
        AudioManager.Instance.PlaySound(AudioConfig.PLANT, transform.position);
        Destroy(transparentImage);
        plant = plantToPlace;
        plant.transform.position = transform.position;
        plant.GetComponent<SpriteRenderer>().sortingLayerName = "Game";
        plant.TransitionToEnable();
        plant.OnPlantDie += RemovePlant;
    }

    private void RemovePlant()
    {
        plant = null;
    }

    public void CancelPlacePlant()
    {
        plantToPlace = null;
        if (transparentImage != null)
        {
            Destroy(transparentImage);
        }
    }
}

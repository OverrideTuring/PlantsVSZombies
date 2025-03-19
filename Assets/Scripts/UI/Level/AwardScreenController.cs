using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AwardScreenController : MonoBehaviour
{
    [SerializeField] private WinObjectData winObjectData;
    private PlantData plantData;
    [SerializeField] private Image plantImage;
    [SerializeField] private TextMeshProUGUI newPlantTip;
    [SerializeField] private TextMeshProUGUI plantName;
    [SerializeField] private TextMeshProUGUI plantDescription;
    private Vector3 originalPosition;

    private void Start()
    {
        winObjectData = LevelManager.Instance.LevelData.winObjectData;
        plantImage.sprite = winObjectData.sourceImage;
        plantDescription.text = winObjectData.description;
        switch (winObjectData.winObjectType)
        {
            case WinObjectType.Money:
                plantData = PlantFactory.Instance.GetPlantData(winObjectData.plantType);
                newPlantTip.text = "You Got Money!";
                plantName.text = "Money Bag";
                break;
            case WinObjectType.ZombieNote:
                plantData = PlantFactory.Instance.GetPlantData(winObjectData.plantType);
                newPlantTip.text = "You Found a Note!";
                plantName.text = "Note";
                break;
            case WinObjectType.Shovel:
                plantData = PlantFactory.Instance.GetPlantData(winObjectData.plantType);
                newPlantTip.text = "You Got the shovel!";
                plantName.text = "Shovel";
                break;
            case WinObjectType.PlantCard:
                plantData = PlantFactory.Instance.GetPlantData(winObjectData.plantType);
                newPlantTip.text = "You Got a New Plant!";
                plantName.text = plantData.plantName;
                break;
            default:
                Debug.LogError("No way, you got nothing!");
                break;
        }
    }

    public void OnNextLevelButtonClick()
    {
        LevelManager.Instance.EnterNextLevel();
    }

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(SceneConfig.MENU_SCENE);
    }

    public void OnPointerEnter(GameObject button)
    {
        button.transform.Find("ButtonGlow").gameObject.SetActive(true);
    }

    public void OnPointerExit(GameObject button)
    {

        button.transform.Find("ButtonGlow").gameObject.SetActive(false);
    }

    public void OnPointerDown(GameObject button)
    {
        AudioManager.Instance.PlaySound2D(AudioConfig.TAP);
        originalPosition = button.transform.localPosition;
        Vector3 newPosition = originalPosition;
        newPosition.x += 2;
        newPosition.y -= 2;
        button.transform.localPosition = newPosition;
    }
    public void OnPointerUp(GameObject button)
    {
        button.transform.localPosition = originalPosition;
    }
}

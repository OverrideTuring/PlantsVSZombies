using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum CardState
{
    None,
    Disable,
    Cooling,
    WaitingSun,
    Ready,
    PickedUp
}

public class Card : MonoBehaviour, IPausable
{
    private CardState cardState = CardState.WaitingSun;
    public PlantType plantType = PlantType.Sunflower;
    
    [SerializeField] private float cdTime = 5;
    private float currentTime = 0;
    [SerializeField] private int neededSunPoints = 50;

    public GameObject cardLight;
    public Image cardMask;
    private CardState pauseCardState;

    private void Start()
    {
        PauseManager.Instance.Register(this);
    }

    void Update()
    {
        switch (cardState) {
            case CardState.Cooling:
                CoolingUpdate();
                break;
            case CardState.WaitingSun:
                WaitingSunUpdate();
                break;
            case CardState.Ready:
                ReadyUpdate();
                break;
            case CardState.PickedUp:
                break;
            default:
                break;
        }
    }
    private void CoolingUpdate()
    {
        currentTime += Time.deltaTime;
        cardMask.fillAmount = (cdTime - currentTime) / cdTime;
        if(currentTime >= cdTime)
        {
            TransitionToWaitingSun();
        }
    }

    private void WaitingSunUpdate()
    {
        if(neededSunPoints <= SunManager.Instance.SunPoints)
        {
            TransitionToReady();
        }
    }
    
    private void ReadyUpdate()
    {
        if(neededSunPoints > SunManager.Instance.SunPoints)
        {
            TransitionToWaitingSun();
        }
    }

    private void TransitionToWaitingSun()
    {
        cardState = CardState.WaitingSun;
        cardLight.GetComponent<Image>().color = Color.gray;
        cardMask.gameObject.SetActive(false);
    }

    private void TransitionToReady()
    {
        cardState = CardState.Ready;
        cardLight.GetComponent<Image>().color = Color.white;
        cardMask.fillAmount = 1f;
        cardMask.gameObject.SetActive(false);
    }
    private void TransitionToCooling()
    {
        cardState = CardState.Cooling;
        currentTime = 0;
        cardMask.fillAmount = 1f;

        cardLight.GetComponent<Image>().color = Color.gray;
        cardMask.gameObject.SetActive(true);
    }

    private void TransitionToPickedUp()
    {
        cardState = CardState.PickedUp;
        cardLight.GetComponent<Image>().color = Color.gray;
        cardMask.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        if (neededSunPoints > SunManager.Instance.SunPoints) return;
        bool success = HandManager.Instance.takePlant(this);
        if(!success) return;
        TransitionToPickedUp();
        AudioManager.Instance.PlaySound2D(AudioConfig.SEED_LIFT);
    }

    public void Disable()
    {
        cardState = CardState.Disable;
        cardLight.GetComponent<Image>().color = Color.gray;
        cardMask.gameObject.SetActive(true);
    }

    public void Enable()
    {
        TransitionToWaitingSun();
    }

    public void AfterPlacePlant()
    {
        SunManager.Instance.UpdateSunPoint(-neededSunPoints);
        TransitionToCooling();
    }

    public void CancelPlacePlant()
    {
        TransitionToWaitingSun();
    }

    public void OnPause()
    {
        pauseCardState = cardState;
        cardState = CardState.None;
    }

    public void OnResume()
    {
        cardState = pauseCardState;
    }
}

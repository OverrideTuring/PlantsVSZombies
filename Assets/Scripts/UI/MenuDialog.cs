using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDialog : MonoBehaviour, ICheckBoxHandler
{
    [SerializeField] private CheckBox accelerationCheckBox;
    [SerializeField] private CheckBox fullScreenCheckBox;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    public void HandleCheckBox(CheckBox checkBox)
    {
        AudioManager.Instance.PlaySound2D(AudioConfig.BUTTON_CLICK);
        if(checkBox == accelerationCheckBox)
        {
            GameConfig.Acceleration = !GameConfig.Acceleration;
        }else if(checkBox == fullScreenCheckBox)
        {
            GameConfig.FullScreen = !GameConfig.FullScreen;
        }
    }

    private void OnEnable()
    {
        GameConfig.OnAccelerationChanged += accelerationCheckBox.UpdateValue;
        GameConfig.OnFullScreenChanged += fullScreenCheckBox.UpdateValue;
    }

    private void OnDisable()
    {
        if(MenuSceneController.Instance != null)
        {
            MenuSceneController.Instance.OnDialogClose();
        }
        GameConfig.OnAccelerationChanged -= accelerationCheckBox.UpdateValue;
        GameConfig.OnFullScreenChanged -= fullScreenCheckBox.UpdateValue;
    }

    private void Awake()
    {
        accelerationCheckBox.handler = this;
        fullScreenCheckBox.handler = this;
    }

    private void Start()
    {
        accelerationCheckBox.value = GameConfig.Acceleration;
        fullScreenCheckBox.value = GameConfig.FullScreen;

        musicSlider.value = GameConfig.MusicVolume;
        soundSlider.value = GameConfig.SoundVolume;
    }

    public void OnSliderValueChanged(Slider slider)
    {
        if(slider == musicSlider)
        {
            GameConfig.MusicVolume = slider.value;
        }else if(slider == soundSlider)
        {
            GameConfig.SoundVolume = slider.value;
        }
    }
}

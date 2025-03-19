using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface ICheckBoxHandler
{
    public void HandleCheckBox(CheckBox checkBox);
}

public class CheckBox : MonoBehaviour
{
    [SerializeField] private Sprite uncheckedSprite;
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Image image;
    private bool _value;
    public bool value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged();
        }
    }
    public ICheckBoxHandler handler;

    private void Awake()
    {
        image = GetComponent<Image>();

        // 添加EventTrigger组件
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if(eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        entry.callback.AddListener((data) => OnPointerDown());
        eventTrigger.triggers.Add(entry);
    }

    public void OnValueChanged()
    {
        if (_value)
        {
            image.sprite = checkedSprite;
        }
        else
        {
            image.sprite = uncheckedSprite;
        }
    }

    public void UpdateValue(bool value)
    {
        this.value = value;
    }

    public void OnPointerDown()
    {
        if(handler == null)
        {
            Debug.LogError("Checkbox未绑定Handler！");
            return;
        }
        handler.HandleCheckBox(this);
    }
}

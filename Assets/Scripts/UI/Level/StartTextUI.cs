using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTextUI : MonoBehaviour
{
    public void OnDisplayOver()
    {
        GameEventManager.Instance.TriggerLevelStart();
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestartLevelButton : BaseButton
{
    public override void OnClick() {
        LevelManager.Instance.RestartLevel();
    }
}

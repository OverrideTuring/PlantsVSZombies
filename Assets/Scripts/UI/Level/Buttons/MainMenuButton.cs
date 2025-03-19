using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : BaseButton
{
    public override void OnClick() {
        LevelManager.Instance.ExitLevel();
    }
}

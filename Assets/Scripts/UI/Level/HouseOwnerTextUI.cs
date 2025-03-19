using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HouseOwnerTextUI : MonoBehaviour
{
    private Animator anim;
    private TextMeshProUGUI text;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        text = GetComponent<TextMeshProUGUI>();
        text.text = "";
        if (PlayerPrefs.HasKey("username"))
        {
            text.text = PlayerPrefs.GetString("username") + "'s House";
        }
    }

    public void Disappear()
    {
        anim.SetTrigger("disappear");
        Destroy(gameObject, 1.0f);
    }
}

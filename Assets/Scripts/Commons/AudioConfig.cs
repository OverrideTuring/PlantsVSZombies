using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfig
{
    public const string AWOOGA = "Audio/Sound/awooga";
    public const string BGM1 = "Audio/Music/Grasswalk";
    public const string BGM2 = "Audio/Music/bgm2";
    public const string BGM3 = "Audio/Music/bgm3";
    public const string BGM4 = "Audio/Music/bgm4";
    public const string BLEEP = "Audio/Sound/bleep";
    public const string BUTTON_CLICK = "Audio/Sound/buttonclick";
    public const string BUZZER = "Audio/Sound/buzzer";
    public const string CHOMP = "Audio/Sound/chomp";
    public const string CHOMP2 = "Audio/Sound/chomp2";
    public const string CHOMP_SOFT = "Audio/Sound/chompsoft";
    public const string EVIL_LAUGH = "Audio/Sound/evillaugh";
    public const string GRAVE_BUTTON = "Audio/Sound/gravebutton";
    public const string GROAN = "Audio/Sound/groan";
    public const string GROAN2 = "Audio/Sound/groan2";
    public const string GROAN3 = "Audio/Sound/groan3";
    public const string GROAN4 = "Audio/Sound/groan4";
    public const string GROAN5 = "Audio/Sound/groan5";
    public const string GROAN6 = "Audio/Sound/groan6";
    public const string GULP = "Audio/Sound/gulp";
    public const string LAST_WAVE = "Audio/Sound/finalwave";
    public const string LIGHT_FILL = "Audio/Sound/lightfill";
    public const string HUGE_WAVE = "Audio/Sound/hugewave";
    public const string KERNEL_PULT = "Audio/Sound/kernelpult2";
    public const string LOSE_MUSIC = "Audio/Sound/losemusic";
    public const string PAUSE = "Audio/Sound/pause";
    public const string PLANT = "Audio/Sound/plant";
    public const string POINTS = "Audio/Sound/points";
    public const string READY_SET_PLANT = "Audio/Sound/readysetplant";
    public const string SCREAM = "Audio/Sound/scream";
    public const string SEED_LIFT = "Audio/Sound/seedlift";
    public const string SHOOT = "Audio/Sound/shoot";
    public const string SIREN = "Audio/Sound/siren";
    public const string SPLAT = "Audio/Sound/splat";
    public const string SPLAT2 = "Audio/Sound/splat2";
    public const string SPLAT3 = "Audio/Sound/splat3";
    public const string TAP = "Audio/Sound/tap";
    public const string THROW = "Audio/Sound/throw";
    public const string THROW2 = "Audio/Sound/throw2";
    public const string WIN_MUSIC = "Audio/Sound/winmusic";

    public static string[] splatList = new string[] { SPLAT, SPLAT2, SPLAT3 };
    public static string[] groanList = new string[] { GROAN, GROAN2, GROAN3, GROAN4, GROAN5, GROAN6 };
    public static string[] chompList = new string[] { CHOMP, CHOMP2, CHOMP_SOFT };

    public static string GetRandomSplat()
    {
        return splatList[Random.Range(0, splatList.Length)];
    }

    public static string GetRandomGroan()
    {
        return groanList[Random.Range(0, groanList.Length)];
    }

    public static string GetRandomChomp()
    {
        return chompList[Random.Range(0, chompList.Length)];
    }
}

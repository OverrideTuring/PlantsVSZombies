using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    private static float _musicVolume = 0.85f;
    private static float _soundVolume = 0.85f;
    private static bool _acceleration = true;
    private static bool _fullScreen = true;
    public static Vector2Int fullScreenRes = new Vector2Int(1920, 1080);
    public static Vector2Int windownScreenRes = new Vector2Int(1440, 1080);

    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSoundVolumeChanged;
    public static event Action<bool> OnAccelerationChanged;
    public static event Action<bool> OnFullScreenChanged;

    static GameConfig()
    {
        OnFullScreenChanged += SetFullScreen;
        SetFullScreen(_fullScreen);
    }

    public static float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            OnMusicVolumeChanged?.Invoke(_musicVolume);
        }
    }

    public static float SoundVolume
    {
        get => _soundVolume;
        set
        {
            _soundVolume = value;
            OnSoundVolumeChanged?.Invoke(_soundVolume);
        }
    }

    public static bool Acceleration
    {
        get => _acceleration;
        set
        {
            _acceleration = value;
            OnAccelerationChanged?.Invoke(_acceleration);
        }
    }

    public static bool FullScreen
    {
        get => _fullScreen;
        set
        {
            _fullScreen = value;
            OnFullScreenChanged?.Invoke(_fullScreen);
        }
    }

    public static void SetFullScreen(bool fullScreen)
    {
        if (fullScreen)
        {
            Screen.SetResolution(fullScreenRes.x, fullScreenRes.y, true);
        }
        else
        {
            Screen.SetResolution(windownScreenRes.x, windownScreenRes.y, false);
        }
    }
}

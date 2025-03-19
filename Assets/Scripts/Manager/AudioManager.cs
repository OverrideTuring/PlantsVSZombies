using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if( _instance == null )
                {
                    GameObject audioManager = new GameObject("AudioManager");
                    audioManager.AddComponent<AudioSource>();
                    _instance = audioManager.AddComponent<AudioManager>();
                    DontDestroyOnLoad(audioManager);
                }
            }
            return _instance;
        }
    }

    private AudioSource musicAudioSource;
    private AudioSource soundAudioSource;
    public float musicVolume = 1.0f;
    public float soundVolume = 1.0f;
    public float alpha = 0.9f;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        soundAudioSource = gameObject.AddComponent<AudioSource>();
        musicVolume = GameConfig.MusicVolume;
        soundVolume = GameConfig.SoundVolume;
        GameConfig.OnMusicVolumeChanged += OnMusicVolumeChanged;
        GameConfig.OnSoundVolumeChanged += OnSoundVolumeChanged;
        DontDestroyOnLoad(gameObject);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = volume;
    }

    private void OnSoundVolumeChanged(float volume)
    {
        soundVolume = volume;
        soundAudioSource.volume = volume;
    }

    public void PlayMusic(string path, bool looping = false, float startTime = 0)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        musicAudioSource.clip = clip;
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = looping;
        musicAudioSource.time = startTime;
        musicAudioSource.Play();
    }

    public bool IsPlaying()
    {
        return musicAudioSource.isPlaying;
    }

    public void FadeOut(float duration)
    {
        if (musicAudioSource.clip == null) return;
        StartCoroutine(Fade(musicAudioSource.volume, 0, duration));
    }

    private IEnumerator Fade(float startVolume, float endVolume, float duration)
    {
        float timer = 0f;
        musicAudioSource.volume = startVolume;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / duration);
            yield return null;
        }

        musicAudioSource.Stop();
    }

    public float PlaySound(string path, Vector3 position)
    {
        // 拉进声音与主摄像机距离
        Vector3 objectToCamera = Camera.main.transform.position - position;
        position += objectToCamera * alpha;

        AudioClip clip = Resources.Load<AudioClip>(path);
        AudioSource.PlayClipAtPoint(clip, position, soundVolume);
        return clip.length;
    }

    public float PlaySound2D(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        soundAudioSource.PlayOneShot(clip, soundVolume);
        // AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, soundVolume);
        return clip.length;
    }

    public void PauseMusic()
    {
        if (!musicAudioSource.isPlaying) return;
        musicAudioSource.Pause();
    }

    public void ContinueMusic()
    {
        musicAudioSource.UnPause();
    }
}

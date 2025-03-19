using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable
{
    public void OnPause();
    public void OnResume();
}

public class PauseManager
{
    private static PauseManager _instance;

    public static PauseManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new PauseManager();
            }
            return _instance;
        }
    }

    private HashSet<IPausable> pausables = new HashSet<IPausable>();

    public void Register(IPausable pausable)
    {
        pausables.Add(pausable);
    }

    public void Unregister(IPausable pausable)
    {
        pausables.Remove(pausable);
    }

    public void Pause()
    {
        LevelManager.Instance.ChangeGameState(GameState.Pausing);
        UIManager.Instance.PutOnBlockingPanel();
        AudioManager.Instance.PauseMusic();
        DOTween.PauseAll();
        foreach(IPausable pausable in pausables)
        {
            if(pausable == null)
            {
                pausables.Remove(pausable);
                continue;
            }
            pausable.OnPause();
        }
    }

    public void Resume()
    {
        LevelManager.Instance.ChangeGameState(GameState.Playing);
        UIManager.Instance.PutOffBlockingPanel();
        AudioManager.Instance.ContinueMusic();
        DOTween.PlayAll();
        foreach (IPausable pausable in pausables)
        {
            if (pausable == null)
            {
                pausables.Remove(pausable);
                continue;
            }
            pausable.OnResume();
        }
    }

}

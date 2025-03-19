using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }
    public event Action OnLevelStart;
    public event Action OnWaveEnd;
    public event Action<Zombie> OnZombieSpawned;
    public event Action<Zombie> OnZombieDie;
    public event Action<Zombie> OnZombieGetIn;
    public event Action<Zombie> OnWinGame;

    private void Awake()
    {
        Instance = this;
    }
    public void TriggerLevelStart()
    {
        OnLevelStart?.Invoke();
    }

    public void TriggerWaveEnd()
    {
        OnWaveEnd?.Invoke();
    }

    public void TriggerZombieSpawned(Zombie zombie)
    {
        OnZombieSpawned?.Invoke(zombie);
    }

    public void TriggerZombieDie(Zombie zombie)
    {
        OnZombieDie?.Invoke(zombie);
    }

    public void TriggerZombieGetIn(Zombie zombie)
    {
        OnZombieGetIn?.Invoke(zombie);
    }

    public void TriggerWinGame(Zombie zombie)
    {
        OnWinGame?.Invoke(zombie);
    }
}

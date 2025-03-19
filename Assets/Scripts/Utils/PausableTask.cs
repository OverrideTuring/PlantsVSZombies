using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PausableTask: IPausable
{
    public bool IsPaused { get; set; }

    public PausableTask()
    {
        if(PauseManager.Instance != null)
        {
            PauseManager.Instance.Register(this);
        }
    }

    public static async Task Delay(int millisecondsDelay, int pauseCheckInterval = 50)
    {
        PausableTask pausableTask = new PausableTask();
        int timer = 0;
        while (timer <= millisecondsDelay)
        {
            await Task.Delay(pauseCheckInterval);
            if (!pausableTask.IsPaused)
            {
                timer += pauseCheckInterval;
            }
        }
        PauseManager.Instance.Unregister(pausableTask);
    }

    public static async void DelayedCall(int millisecondsDelay, Action callback, int pauseCheckInterval = 50)
    {
        PausableTask pausableTask = new PausableTask();
        float timer = 0;
        while (timer <= millisecondsDelay)
        {
            await Task.Delay(pauseCheckInterval);
            if (!pausableTask.IsPaused)
            {
                timer += pauseCheckInterval;
            }
        }
        callback();
        PauseManager.Instance.Unregister(pausableTask);
    }

    public void OnPause()
    {
        IsPaused = true;
    }

    public void OnResume()
    {
        IsPaused = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableWaitForSeconds : IEnumerator, IPausable
{
    private readonly float _targetDuration;   // 目标总时长
    private float _elapsed;                   // 已过去的时间
    private bool _useUnscaledTime;            // 是否使用非缩放时间
    public bool IsPaused { get; set; }

    public PausableWaitForSeconds(float seconds, bool useUnscaledTime = false)
    {
        _targetDuration = seconds;
        _useUnscaledTime = useUnscaledTime;
        if(PauseManager.Instance != null)
        {
            PauseManager.Instance.Register(this);
        }
    }

    public object Current => null;

    public bool MoveNext()
    {
        if (IsPaused) return true;

        // 根据设置选择时间增量
        float delta = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        _elapsed += delta;
        return _elapsed < _targetDuration;
    }

    public void Reset()
    {
        _elapsed = 0;
        IsPaused = false;
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

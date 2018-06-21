using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickEvent
{
    int _time;
    int _max;
    UnityEvent OnTime = new UnityEvent();
    public TickEvent(int max)
    {
        _max = max;
    }
    public void AddListener(UnityAction cb)
    {
        OnTime.AddListener(cb);
    }
    public void Update()
    {
        _time++;
        if (_time > _max)
        {
            _time = 0;
            OnTime.Invoke();
        }
    }
}
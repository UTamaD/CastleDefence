using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff
{
    public float Duration { get; private set; }
    public int StackCount { get; private set; }
    
    public SlowDebuff(float duration)
    {
        Duration = duration;
        StackCount = 1;
    }

    public void Refresh(float duration)
    {
        Duration = Mathf.Max(Duration, duration);
        StackCount = Mathf.Min(StackCount + 1, 3);
    }

    public void Update(float deltaTime)
    {
        Duration -= deltaTime;
    }

    public bool IsExpired()
    {
        
        return Duration <= 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff
{
    public float Duration { get; private set; }
    public int StackCount { get; private set; }
    
    /// <summary>
    /// 슬로우 디버프 생성자
    /// </summary>
    /// <param name="duration">디버프 지속 시간</param>
    public SlowDebuff(float duration)
    {
        // 초기 지속 시간과 스택 수 설정
        Duration = duration;
        StackCount = 1;
    }

    /// <summary>
    /// 디버프 지속 시간을 갱신하고 스택 수를 증가시키는 함수
    /// </summary>
    /// <param name="duration">새로운 디버프 지속 시간</param>
    public void Refresh(float duration)
    {
        // 지속 시간은 더 긴 시간으로 갱신
        Duration = Mathf.Max(Duration, duration);
        // 스택 수는 최대 3개까지 증가
        StackCount = Mathf.Min(StackCount + 1, 3);
    }

    /// <summary>
    /// 디버프 지속 시간을 업데이트하는 함수
    /// </summary>
    /// <param name="deltaTime">경과 시간</param>
    public void Update(float deltaTime)
    {
        // 지속 시간 감소
        Duration -= deltaTime;
    }

    /// <summary>
    /// 디버프가 만료되었는지 확인하는 함수
    /// </summary>
    /// <returns>디버프가 만료되었으면 true, 아니면 false</returns>
    public bool IsExpired()
    {
        // 지속 시간이 0 이하면 만료된 것으로 판단
        return Duration <= 0;
    }
}
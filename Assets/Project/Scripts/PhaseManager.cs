using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PhaseState
{
    NoneMyState,
    Phase1_Ready,
    Phase1_Running,
    Phase2_Ready,
    Phase2_Running,
    Phase3_Ready,
    Phase3_Running,
    PhaseBoss_Ready,
    PhaseBoss_Running
}

public class PhaseManager : SceneSingleton<PhaseManager>
{
    public List<Transform> _destinations = new();
    public int CurrentPhase { get; private set; } = 1;

    public (int, Vector3) GetDestination(int index)
    {
        int resultIndex = index;
        if (resultIndex >= _destinations.Count)
        {
            resultIndex = 0;
        }

        return (resultIndex, _destinations[resultIndex].position);
    }

    public void AdvanceToNextPhase()
    {
        CurrentPhase++;
        if (CurrentPhase > 4) // 4는 보스 단계
        {
            CurrentPhase = 1; // 게임 재시작 또는 다른 로직 추가
        }
        MyPlayerController.Instance.SetCurrentPhase(CurrentPhase);
    }
}
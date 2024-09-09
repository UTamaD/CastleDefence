using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Monster1_LoopingMove : VMyState<FSM_Monster1State>
{
    public override FSM_Monster1State StateEnum => FSM_Monster1State.FSM_Monster1_LoopingMove;
    private Monster1 monster1;

    protected override void Awake()
    {
        base.Awake();
        monster1 = GetComponent<Monster1>();
    }

    protected override void EnterState()
    {
    }

    protected override void ExcuteState()
    {
    }

    protected override void ExcuteState_FixedUpdate()
    {
        (int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(monster1.DestinationIndex);
        monster1.DestinationIndex = destinationInfo.Item1;
        if (monster1.MoveToDestination(destinationInfo.Item2))
        {
            monster1.DestinationIndex++;
        }
    }

    protected override void ExcuteState_LateUpdate()
    {
    }

    protected override void ExitState()
    {
    }


}

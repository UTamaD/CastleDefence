using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Monster1_Stun : VMyState<FSM_Monster1State>
{
    public override FSM_Monster1State StateEnum { get; }
    
    protected override void Awake()
    {
        base.Awake();
        
        
    }
    
    protected override void EnterState()
    {
        //actionStunInsance.EnterAction();
    }

    protected override void ExcuteState()
    {
        //actionStunInsance.ExitAction();
    }

    protected override void ExitState()
    {
        //actionStunInsance.ExitAction();
    }
}

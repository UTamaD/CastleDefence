using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FSM_Monster1State
{
    None, 
    FSM_Monster1_LoopingMove,
    FSM_Monster1_Stun,
    FSM_Monster1_Death
}
public class FSM_Monster1 : StateMachine<FSM_Monster1State>
{
}

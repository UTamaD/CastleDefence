using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FSM_Character1State
{
    None, 
    FSM_Character1State_Idle,
    FSM_Character1State_MoveToDestination,
    FSM_Character1State_Skill1,
    FSM_Character1State_Skill2,
    FSM_Character1State_Skill3
}

public class FSM_Character1 : StateMachine<FSM_Character1State>
{
    
}

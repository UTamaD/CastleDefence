using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Character1State_Idle :  VMyState<FSM_Character1State>
{
    public override FSM_Character1State StateEnum => FSM_Character1State.FSM_Character1State_Idle;
    
    private Character1 _character1;
    
    protected override void Awake()
    {
        base.Awake();

        _character1 = GetComponent<Character1>();
    }

    protected override void EnterState()
    {
        
    }

    protected override void ExcuteState()
    {
        foreach (var skillInstance in _character1.skillInstances)
        {
            foreach (var instanceMonster in MyPlayerController.Instance.GetAliveMonsterList())
            {
                if (skillInstance.IsCooltiming())
                    continue;
                
                if (skillInstance.info.AttackDistance >=
                    (instanceMonster.transform.position - _character1.transform.position).magnitude && !instanceMonster.GetComponent<Monster1>().IsDeadOrStun() )
                {
                    skillInstance.target = instanceMonster;
                    
                    
                    if (skillInstance.info.SkillName == "PriMagic") 
                    {
                        _character1.StartSkillPrimaryAttack(skillInstance);
                    }
                    else if (skillInstance.info.SkillName == "AOEMagic")
                    {
                        _character1.StartSkillAreaOfEffect(skillInstance);
                    }
                    else if(skillInstance.info.SkillName == "StunMagic")
                    {
                        _character1.StartSkillStun(skillInstance);
                    }
                    return;
                }
            }
        }
    }

    protected override void ExitState()
    {
    }
}

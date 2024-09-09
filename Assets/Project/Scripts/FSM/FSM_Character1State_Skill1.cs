using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FSM_Character1State_Skill1 :  VMyState<FSM_Character1State>
{
    public override FSM_Character1State StateEnum => FSM_Character1State.FSM_Character1State_Skill1;

    private Animator _animator;
    private Character1 _character1;
    private Rigidbody _rb;

   

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
        _character1 = GetComponent<Character1>();
        _rb = GetComponent<Rigidbody>();
    }

    protected override void EnterState()
    {   
        SoundManager.Instance.PlaySFX(_character1.activeSkillInstance.info.sfx_name);
        _character1.activeSkillInstance.StartCooltime();
        _animator.CrossFade(_character1.activeSkillInstance.info.AnimationName_Hash, 0.0f);
        StartCoroutine(AnimationFinishCheck());
    }

    protected override void ExcuteState()
    {
        
    }

    protected override void ExitState()
    {
    }

    protected override void ExcuteState_FixedUpdate()
    {
        base.ExcuteState_FixedUpdate();

        if (_character1.activeSkillInstance.target)
        {
           
            _rb.MoveRotation(Quaternion.LookRotation((_character1.activeSkillInstance.target.transform.position - transform.position).normalized));
        }
    }

    IEnumerator AnimationFinishCheck()
    {
        
        yield return null;
        while (true)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash != _character1.activeSkillInstance.info.AnimationName_Hash)
            {
                break;
            }

            yield return null;
        }
        
        
        if (_character1.activeSkillInstance.target != null)
        {
            
            Monster1 targetMonster = _character1.activeSkillInstance.target.GetComponent<Monster1>();
            if (targetMonster != null)
            {
                targetMonster.ApplySlowDebuff(_character1.activeSkillInstance.info.DebuffDuration);
                targetMonster.TakeDamage(_character1.activeSkillInstance.info.Damage,_character1.activeSkillInstance.info.DamageType);
            }
        }
        
        _character1.Fsm.ChangeState(FSM_Character1State.FSM_Character1State_Idle);
    }
}

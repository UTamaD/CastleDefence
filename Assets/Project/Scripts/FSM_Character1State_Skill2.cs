using UnityEngine;
using System.Collections;

public class FSM_Character1State_Skill2 : VMyState<FSM_Character1State>
{
    public override FSM_Character1State StateEnum => FSM_Character1State.FSM_Character1State_Skill2;

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
        SoundManager.Instance.PlaySFX("Thunder");
        _character1.activeSkillInstance.StartCooltime();
        _animator.CrossFade(_character1.activeSkillInstance.info.AnimationName_Hash, 0.0f);
        StartCoroutine(PerformSkill2());
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

    IEnumerator PerformSkill2()
    {
        yield return new WaitForSeconds(0.5f); // 애니메이션 시작 후 대기 시간

        if (_character1.activeSkillInstance.target != null)
        {
            GameObject projectileObj = Instantiate(_character1.activeSkillInstance.info.ProjectilePrefab, 
                _character1.transform.position + Vector3.up, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.target = _character1.activeSkillInstance.target;
                projectile.damage = _character1.activeSkillInstance.info.Damage;
            }
        }

        yield return new WaitForSeconds(0.5f); // 추가 대기 시간

        _character1.Fsm.ChangeState(FSM_Character1State.FSM_Character1State_Idle);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Character1State_Skill3 : VMyState<FSM_Character1State>
{
    public override FSM_Character1State StateEnum => FSM_Character1State.FSM_Character1State_Skill3;

    private Character1 _character1;
    private Animator _animator;
    private Coroutine _skillCoroutine;
    private Rigidbody _rb;

    private Transform lookAt;
    private bool isCast;
    private GameObject aoeObj;
    protected override void Awake()
    {
        base.Awake();
        _character1 = GetComponent<Character1>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    protected override void EnterState()
    {

        
        _character1.activeSkillInstance.StartCooltime();
        _animator.CrossFade(_character1.activeSkillInstance.info.AnimationName_Hash, 0.0f);
        _skillCoroutine = StartCoroutine(PerformSkill3());
    }

    protected override void ExitState()
    {
        if (_skillCoroutine != null)
        {
            StopCoroutine(_skillCoroutine);
        }

        aoeObj = null;
    }

    protected override void ExcuteState_FixedUpdate()
    {
        base.ExcuteState_FixedUpdate();

        if (aoeObj != null )
        {
            lookAt = aoeObj.GetComponent<Transform>();
            
            if (aoeObj)
            {
                _rb.MoveRotation(Quaternion.LookRotation((lookAt.position- transform.position).normalized));
            }
            
        }
        else
        {
            _animator.SetBool("isCasting",false);
        }
       
        

    }
    
    IEnumerator PerformSkill3()
    {
        while (true)
        {
            GameObject target = FindTargetInRange();
            if (target != null)
            {
                //isCast = true;
                CreateAreaOfEffect(target.transform.position);
                yield return new WaitForSeconds(_character1.activeSkillInstance.info.AreaOfEffectDuration);
                //isCast = false;
            }
            else
            {
                //isCast = false;
                yield return new WaitForSeconds(1f);
                
            }
        }
    }

    private GameObject FindTargetInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _character1.activeSkillInstance.info.AttackDistance);
        List<GameObject> validTargets = new List<GameObject>();

        foreach (var hitCollider in hitColliders)
        {
            Monster1 monster = hitCollider.GetComponent<Monster1>();
            if (monster != null && monster.IsAlive())
            {
                validTargets.Add(monster.gameObject);
            }
        }

        if (validTargets.Count > 0)
        {
            return validTargets[Random.Range(0, validTargets.Count)];
        }

        return null;
    }

    private void CreateAreaOfEffect(Vector3 position)
    {
        
        SoundManager.Instance.PlaySFX("Fire");
        position = new Vector3(position.x, 5, position.z);
        aoeObj = Instantiate(_character1.activeSkillInstance.info.AreaOfEffectPrefab, position, Quaternion.identity);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();
        _animator.SetBool("isCasting",true);
        if (aoe != null)
        {
            aoe.radius = _character1.activeSkillInstance.info.AreaOfEffectRadius;
            aoe.damage = _character1.activeSkillInstance.info.AreaOfEffectDamage;
            aoe.duration = _character1.activeSkillInstance.info.AreaOfEffectDuration;
            aoe.damageInterval = _character1.activeSkillInstance.info.AreaOfEffectDamageInterval;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character1 : CharacterBase<FSM_Character1>
{
    public List<SkillInfo> SkillInfos;
    public List<SkillInstance> skillInstances;
    public SkillInstance activeSkillInstance;
    //public Vector3 Destination;

    public int level = 0;
    

    protected override void Awake()
    {
        base.Awake();
        skillInstances = new List<SkillInstance>();
        foreach (var skillInfo in SkillInfos)
        {
            SkillInstance inst = gameObject.AddComponent<SkillInstance>();
            inst.info = Instantiate(skillInfo);
            skillInstances.Add(inst);
        }
        
        foreach (var skillInstance in skillInstances)
        {
            skillInstance.info.Damage *= Mathf.Pow(1 + UpgradeManager.Instance.towerDamageIncrease, UpgradeManager.Instance.towerDamageUpgrades);
        }
    }

    public void StartSkillStun(SkillInstance instance)
    {
        activeSkillInstance = instance;
        Fsm.ChangeState(FSM_Character1State.FSM_Character1State_Skill1);
    }

    public void StartSkillPrimaryAttack(SkillInstance instance)
    {
        activeSkillInstance = instance;
        Fsm.ChangeState(FSM_Character1State.FSM_Character1State_Skill2);
    }
    
    public void StartSkillAreaOfEffect(SkillInstance instance)
    {
        activeSkillInstance = instance;
        Fsm.ChangeState(FSM_Character1State.FSM_Character1State_Skill3);
    }
    
    /*
    public void SetDestination(Vector3 destination)
    {
        //Destination = destination;
        Fsm.ChangeState(FSM_Character1State.FSM_Character1State_MoveToDestination);
    }
    */
}

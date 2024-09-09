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


    public GameObject towerModel;
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
    
    public void UpgradeMaterial()
    {

        float newValue = level * 0.1f;
        newValue = Mathf.Clamp(newValue, 0, 1.0f);

        Material material = towerModel.gameObject.GetComponent<Renderer>().material;
        material.SetFloat("_Smoothness", newValue);
        material.SetFloat("_Metallic", newValue);
        
        Color outerChlothesColor = material.GetColor("_OuterChlothes");
        float newColorValue = Mathf.Lerp(255f, 105f, level / 10f) / 255f;
        
        if (outerChlothesColor.r > 0) outerChlothesColor.r = newColorValue;
        if (outerChlothesColor.g > 0) outerChlothesColor.g = newColorValue;
        if (outerChlothesColor.b > 0) outerChlothesColor.b = newColorValue;
        
        material.SetColor("_OuterChlothes", outerChlothesColor);
        
    }
    
    
    
}

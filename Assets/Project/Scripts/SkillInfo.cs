using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillInfo", menuName = "Skill/CreateSkillInfo")]
public class SkillInfo : ScriptableObject
{
    public string SkillName; 
    public float AttackDistance;
    public float Cooltime;

    public string AnimationName;
    public float Damage; 
    [NonSerialized] public int AnimationName_Hash;
    public float DebuffDuration;
    
    public GameObject ProjectilePrefab;
    
    public GameObject AreaOfEffectPrefab;
    public float AreaOfEffectDuration = 10f;
    public float AreaOfEffectRadius = 5f;
    public float AreaOfEffectDamage = 10f;
    public float AreaOfEffectDamageInterval = 1f;
    
    void OnEnable()
    {
        AnimationName_Hash = Animator.StringToHash(AnimationName);
    }
}

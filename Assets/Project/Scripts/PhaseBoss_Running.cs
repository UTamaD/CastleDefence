using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
public class PhaseBoss_Running : VMyState<PhaseState>
{
    public int SpawnMonsterCount = 10;
    public float Interval = 1.0f;
    
    public bool isSpawn;
    public override PhaseState StateEnum => PhaseState.PhaseBoss_Running;
    
    private bool isTimeEnd = false;

    public float PhaseTime = 30f;
    
    IEnumerator TimeOut()
    {
        
        yield return new WaitForSeconds(PhaseTime);
        isTimeEnd = true;
    }
    
    public override bool IsPhaseComplete()
    {
        return !isSpawn && (MyPlayerController.Instance.GetMonsterList().Count == 0 || isTimeEnd);
    }


    protected override void EnterState()
    {
        isSpawn = true;
        Debug.Log("Enter Boss Running");
        StartCoroutine(TimeOut());
        StartCoroutine(SpawnMonsters());
    }

    
    IEnumerator SpawnMonsters()
    {
        for (int i = 0; i < SpawnMonsterCount; ++i)
        {
            MyPlayerController.Instance.GetNewMonster(transform.position, Quaternion.LookRotation(Vector3.right));
            yield return new WaitForSeconds(Interval);
            
        }
        isSpawn = false;
        
    }

    protected override void ExcuteState()
    {
      
        if (!isSpawn)
        {
            
            OwnerStateMachine.ChangeState(PhaseState.Phase3_Ready);
        }
        
    }

    protected override void ExitState()
    {
        //var data = GetEnumValue<FSM_Monster1State>();
    }

    public T GetEnumValue<T>() where T : Enum
    {
        throw new NotImplementedException();
    }
    

}

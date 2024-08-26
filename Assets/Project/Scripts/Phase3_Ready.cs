using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Phase3_Ready : VMyState<PhaseState>
{
    public override PhaseState StateEnum => PhaseState.Phase3_Ready;
    public string stageName;
    protected override void EnterState()
    {
        Debug.Log(stageName);
        UIManager.Instance.ShowStageText(stageName);
        StartCoroutine(GoToNextState());
    }

    IEnumerator GoToNextState()
    {
        yield return new WaitForSeconds(3.0f);
        OwnerStateMachine.ChangeState(PhaseState.Phase3_Running);
       
    }
    
    protected  override void ExitState()
    {
    }
}

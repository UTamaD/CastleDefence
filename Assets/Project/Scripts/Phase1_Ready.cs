using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Phase1_Ready : VMyState<PhaseState>
{
    public override PhaseState StateEnum => PhaseState.Phase1_Ready;
    public string stageName;

    protected override void EnterState()
    {
        Debug.Log(stageName);
        UIManager.Instance.ShowStageText(stageName);
        StopAllCoroutines();
        StartCoroutine(GoToNextState());
    }

    IEnumerator GoToNextState()
    {
        Debug.Log("test");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("test2");
        OwnerStateMachine.ChangeState(PhaseState.Phase1_Running);
       
    }

    protected  override void ExcuteState()
    {


    }

    protected  override void ExitState()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBoss_Ready : VMyState<PhaseState>
{
    public override PhaseState StateEnum => PhaseState.PhaseBoss_Ready;
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
        OwnerStateMachine.ChangeState(PhaseState.PhaseBoss_Running);
       
    }

    protected  override void ExcuteState()
    {


    }

    protected  override void ExitState()
    {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Phase1_Ready : VMyState<PhaseState>
{
    public override PhaseState StateEnum => PhaseState.Phase1_Ready;
    public string stageName;
    private Coroutine goToNextStateCoroutine;

    protected override void EnterState()
    {
        UIManager.Instance.ShowStageText(stageName);
        if (goToNextStateCoroutine != null)
        {
            StopCoroutine(goToNextStateCoroutine);
        }
        goToNextStateCoroutine = StartCoroutine(GoToNextState());
    }

    protected override void ExitState()
    {
        if (goToNextStateCoroutine != null)
        {
            StopCoroutine(goToNextStateCoroutine);
            goToNextStateCoroutine = null;
        }
    }

    IEnumerator GoToNextState()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(3.0f);
        OwnerStateMachine.ChangeState(PhaseState.Phase1_Running);

    }

    protected  override void ExcuteState()
    {


    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseStateMachine : StateMachine<PhaseState>
{
    private PhaseManager phaseManager;

    protected override void Start()
    {
        base.Start();
        phaseManager = PhaseManager.Instance;
    }

    protected void Update()
    {
       
        CheckPhaseChange();
    }

    private void CheckPhaseChange()
    {
        
        if (_currentMyState is Phase1_Running || _currentMyState is Phase2_Running || _currentMyState is Phase3_Running|| _currentMyState is PhaseBoss_Running)
        {
            var runningState = _currentMyState as VMyState<PhaseState>;
            if (runningState != null && (runningState.IsPhaseComplete()))
            {
                phaseManager.AdvanceToNextPhase();
                SwitchToNextPhase();
            }
        }
    }

    private void SwitchToNextPhase()
    {
        switch (phaseManager.CurrentPhase)
        {
            case 1:
                ChangeState(PhaseState.Phase1_Ready);
                break;
            case 2:
                ChangeState(PhaseState.Phase2_Ready);
                break;
            case 3:
                ChangeState(PhaseState.Phase3_Ready);
                break;
            case 4:
                ChangeState(PhaseState.PhaseBoss_Ready);
                break;
        }
    }
}

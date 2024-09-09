using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseStateMachine : StateMachine<PhaseState>
{
    

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded, initializing PhaseStateMachine");
        InitializeStateMachine();
    }
    public void InitializeStateMachine()
    {
        Debug.Log("Initializing PhaseStateMachine");
        ChangeState(PhaseState.Phase1_Ready);
    }
    
    protected override void Start()
    {
        base.Start();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeStateMachine();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public new void ChangeState(PhaseState newState)
    {
 
        base.ChangeState(newState);
        Debug.Log($"State changed to {newState}");
    }

    
    
    protected void Update()
    {
        if (_currentMyState == null)
        {
            Debug.LogError("Current state is null");
            InitializeStateMachine();
        }
        CheckPhaseChange();
    }

    private void CheckPhaseChange()
    {
        
        if (_currentMyState is Phase1_Running || _currentMyState is Phase2_Running ||
            _currentMyState is Phase3_Running|| _currentMyState is PhaseBoss_Running)
        {
            var runningState = _currentMyState as VMyState<PhaseState>;
            if (runningState != null && (runningState.IsPhaseComplete()))
            {
                PhaseManager.Instance.AdvanceToNextPhase();
                SwitchToNextPhase();
            }
        }
    }

    private void SwitchToNextPhase()
    {
        switch (PhaseManager.Instance.CurrentPhase)
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

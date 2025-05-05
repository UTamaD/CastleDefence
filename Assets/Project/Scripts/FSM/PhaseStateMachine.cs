using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseStateMachine : StateMachine<PhaseState>
{
    

    /// <summary>
    /// 씬이 로드될 때 호출되는 이벤트 함수
    /// </summary>
    /// <param name="scene">로드된 씬</param>
    /// <param name="mode">씬 로드 모드</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded, initializing PhaseStateMachine");
        InitializeStateMachine();
    }

    /// <summary>
    /// 상태 머신을 초기화하는 함수
    /// </summary>
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
    
    /// <summary>
    /// 새로운 상태로 변경하는 함수
    /// </summary>
    /// <param name="newState">변경할 새 상태</param>
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

    /// <summary>
    /// 페이즈 변경 조건을 체크하는 함수
    /// </summary>
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

    /// <summary>
    /// 다음 페이즈로 전환하는 함수
    /// </summary>
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

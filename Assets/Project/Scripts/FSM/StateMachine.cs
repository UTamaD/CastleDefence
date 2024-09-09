using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class NotifyBase
{
    
}

public interface IVMyState
{
    void OnNotify<T, T2>(T eValue, T2 vValue) where T : Enum where T2 : NotifyBase;
    
    public void EnterStateWrapper();
    public void ExcuteStateWrapper();

    public void ExcuteState_FixedUpdateWrapper();

    public void ExcuteState_LateUpdateWrapper();

    public void ExitStateWrapper();

    public void AddState<T>(StateMachine<T> owner, ref object _states) where T : Enum;
}

public abstract class VMyStateBase : MonoBehaviour
{
}

public abstract class VMyState<T> : VMyStateBase, IVMyState where T : Enum
{
    
    public abstract T StateEnum { get; }
    
    [NonSerialized]public StateMachine<T> OwnerStateMachine;

    
    public  StateMachine<T> HSFM_StateMachine;

    protected virtual void Awake()
    {
    }
    
    protected virtual void Start()
    {
    }

    public void OnNotify<T1, T2>(T1 eValue, T2 vValue) where T1 : Enum where T2 : NotifyBase
    {
        throw new NotImplementedException();
    }

    public void EnterStateWrapper()
    {
        Debug.Log($"Entering state: {StateEnum}");
        EnterState();
    }

    public void ExcuteStateWrapper()
    {
        ExcuteState();
    }
    
    public void ExcuteState_FixedUpdateWrapper()
    {
        ExcuteState_FixedUpdate();
    }
    
    public void ExcuteState_LateUpdateWrapper()
    {
        ExcuteState_LateUpdate();
    }

    public void ExitStateWrapper()
    {
        Debug.Log($"Exiting state: {StateEnum}");
        ExitState();
        if (HSFM_StateMachine)
        {
            HSFM_StateMachine.ChangeState((T)Enum.Parse(typeof(T), "None"));
        }
    }

    public void AddState<T1>(StateMachine<T1> owner, ref object _states) where T1 : Enum
    {
        var cast =_states as SerializedDictionary<T, IVMyState>;
        OwnerStateMachine = owner as StateMachine<T>;
        cast?.Add(StateEnum, this);
    }

    protected virtual void EnterState()
    {
        
    }

    protected virtual void ExcuteState()
    {
        
    }

    protected virtual void ExitState()
    {
        
    }
    
    protected virtual void ExcuteState_FixedUpdate()
    {
        
    }

    protected virtual void ExcuteState_LateUpdate()
    {
        
    }
    
    public virtual bool IsPhaseComplete()
    {
        return false;
    }
}

public class StateMachine<T> : MonoBehaviour where T : Enum
{
    [SerializeField] private T defaultState;
    
    [SerializeField] protected IVMyState _currentMyState;
    private SerializedDictionary<T, IVMyState> _states = new();
    
    StateMachine<T> GetSuperOwnerStateMachile()
    {
        StateMachine<T> stateMachine = GetComponentInParent< StateMachine<T>>();
        if (stateMachine)
        {
            return stateMachine.GetSuperOwnerStateMachile();
        }

        return this;
    }
    
    private void ChangeState_Internal(IVMyState newMyState)
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExitStateWrapper();
        }

        if (newMyState == null)
        {
            _currentMyState = null;
            return;
        }

        _currentMyState = newMyState;
        _currentMyState.EnterStateWrapper();
    }

    public void OnNotify<T1, T2>(T1 eValue, T2 vValue) where T1 : Enum where T2 : NotifyBase
    {
        _currentMyState.OnNotify(eValue, vValue);
    }

    public void ChangeState(T state)
    {
        // 상태가 None이 아니면 돌릴 상태가 있으므로 Active
        if (_states.TryGetValue(state, out var newState))
        {
            ChangeState_Internal(newState);
        }
        else
        {
            Debug.LogError($"State {state} not found in the state machine.");
            ChangeState_Internal(null);
        }
    }
    
    // Start is called before the first frame update
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        // 이거는 성능이 직접 컴포넌트 가져오는 방식 대비 비싸다.
        var stateArray = GetComponents<VMyStateBase>().OfType<IVMyState>().ToList();
        foreach (var state in stateArray)
        {
            object states = _states;
            state.AddState(this, ref states);
        }

        // DefaultState
        ChangeState(defaultState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteStateWrapper();
        }
    }

    private void FixedUpdate()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteState_FixedUpdateWrapper();
        }
    }

    private void LateUpdate()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteState_LateUpdateWrapper();
        }
    }
}
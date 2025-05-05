using UnityEngine;

public class FSM_Monster1_Death : VMyState<FSM_Monster1State>
{
    public override FSM_Monster1State StateEnum => FSM_Monster1State.FSM_Monster1_Death;

    protected override void EnterState()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject);
    }

    protected override void ExcuteState()
    {

    }

    protected override void ExitState()
    {

    }
}
using UnityEngine;

public class FSM_Monster1_Death : VMyState<FSM_Monster1State>
{
    public override FSM_Monster1State StateEnum => FSM_Monster1State.FSM_Monster1_Death;

    protected override void EnterState()
    {
        // 사망 상태에 진입할 때 필요한 작업 수행
        // 예: 콜라이더 비활성화, 움직임 정지 등
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject);
    }

    protected override void ExcuteState()
    {
        // 사망 상태에서는 특별한 작업을 수행하지 않음
    }

    protected override void ExitState()
    {
        // 사망 상태에서 빠져나갈 일은 없지만, 혹시 모를 상황을 대비해 구현
    }
}
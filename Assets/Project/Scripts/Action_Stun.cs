using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Stun : ActionBase
{
    private Monster1 monster1;
    private Coroutine _coroutine;
    
    public Action_Stun(GameObject owner) : base(owner)
    {
        monster1 = owner.GetComponent<Monster1>();
    }

    public override void EnterAction()
    {
        
        base.EnterAction();
        _coroutine = monster1.StartCoroutine(Stun());
    }

    public override void ExitAction()
    {
        base.ExitAction();

        if (_coroutine != null)
        {
            monster1.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    IEnumerator Stun()
    {
        
        yield return new WaitForSeconds(5.0f);
        _coroutine = null;
        monster1.OnStunFInish();
    }
}

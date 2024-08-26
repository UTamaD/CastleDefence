using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase<T> : DefenceBehaviorBase
{
    public T Fsm;
    protected Rigidbody _rb;

    protected virtual void Awake()
    {
        Fsm = GetComponent<T>();
        _rb = GetComponent<Rigidbody>();
    }
}

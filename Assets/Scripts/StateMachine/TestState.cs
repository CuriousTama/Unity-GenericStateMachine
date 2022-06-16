using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : IState
{
    public void Enter(StateMachine stateMachine)
    {
        Debug.Log("Enter");
    }

    public void Exit(StateMachine stateMachine)
    {
        Debug.Log("Exit");
    }


    public void PreUpdate(StateMachine stateMachine)
    {
    }

    public void Update(StateMachine stateMachine)
    {
    }

    public void LateUpdate(StateMachine stateMachine)
    {
    }

    public void FixedUpdate(StateMachine stateMachine)
    {
    }
}

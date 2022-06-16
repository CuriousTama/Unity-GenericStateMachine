using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerState : State
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PreUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PreformAction();
    }

    public override void Update()
    {
        Debug.Log("Via input manager");
    }

    private void PreformAction()
    {
        stateMachine.ChangeState(typeof(InputSystemState));
    }
}

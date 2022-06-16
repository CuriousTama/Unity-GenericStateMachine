using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputSystemState : State
{
    private Inputs inputs = new Inputs();

    public override void RegisterInput()
    {
        inputs.StateMachineTests.Enable();
        inputs.StateMachineTests.Space.performed += PreformAction;
    }

    public override void UnregisterInput()
    {
        inputs.StateMachineTests.Disable();
        inputs.StateMachineTests.Space.performed -= PreformAction;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        Debug.Log("Via input system");
    }


    private void PreformAction(InputAction.CallbackContext ctx)
    {
        stateMachine.ChangeState(typeof(InputManagerState));
    }
}

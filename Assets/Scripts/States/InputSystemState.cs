using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class InputSystemState : State 
{
    private Inputs inputs = new Inputs();
    private TextMeshProUGUI text;

    public override void Init()
    {
        text = stateMachine.variables.Get<TextMeshProUGUI>("Text");
    }

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
        text.text = "Input System State";
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
    }


    private void PreformAction(InputAction.CallbackContext ctx)
    {
        stateMachine.ChangeState(typeof(InputManagerState));
    }
}

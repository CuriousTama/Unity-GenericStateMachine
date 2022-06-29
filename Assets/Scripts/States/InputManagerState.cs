using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputManagerState : State
{
    private TextMeshProUGUI text;

    public override void Init()
    {
        text = stateMachine.variables.Get<TextMeshProUGUI>("Text");
    }

    public override void Enter()
    {
        base.Enter();
        text.text = "Input Manager State";
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
    }

    private void PreformAction()
    {
        stateMachine.ChangeState(typeof(InputSystemState));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericStateMachine;

public class StackMotherState : State
{
    private float timer = 0f;

    public override void Update()
    {
        timer += Time.deltaTime;
        stateMachine.variables.Get<TMPro.TMP_Text>("TimeText").text = timer.ToString("0") + " seconds";
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.variables.Get<TMPro.TMP_Text>("TimeText").text = "-- seconds";
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
    }
}

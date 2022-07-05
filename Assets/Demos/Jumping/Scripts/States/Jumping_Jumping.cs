using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericStateMachine;

public class Jumping_Jumping : State
{
    private JumpingStuff data;
    private float time = 0;

    public override void Enter()
    {
        base.Enter();
        data = stateMachine.variables.Get<JumpingStuff>("Data");

        data.stateText.text = "Jump";
        data.animator.SetTrigger("JumpStart");
        data.jumped = true;
    }

    public override void LateUpdate()
    {
        time += Time.deltaTime;

        if (time <= 0.2f)
            return;

        data.rb.AddForce(Vector3.up * data.jumpPower, ForceMode.Impulse);
        stateMachine.ChangeState(typeof(Jumping_Falling));
    }

    public override void Exit()
    {
        base.Exit();
    }
}

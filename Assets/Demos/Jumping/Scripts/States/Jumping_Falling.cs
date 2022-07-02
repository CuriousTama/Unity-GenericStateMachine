using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_Falling : State
{
    private JumpingStuff data;
    private float timeFromJump = 0.25f;

    public override void Enter()
    {
        base.Enter();
        data = stateMachine.variables.Get<JumpingStuff>("Data");
        data.stateText.text = "falling";

        if (data.jumped == false)
        {
            data.animator.SetTrigger("Falling");
            timeFromJump = 0f;
        }
    }

    public override void Update()
    {
        timeFromJump -= Time.deltaTime;

        if (data.groundCheck.isGrounded && timeFromJump <= 0f)
        {
            stateMachine.ChangeState(typeof(Jumping_Landing));
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

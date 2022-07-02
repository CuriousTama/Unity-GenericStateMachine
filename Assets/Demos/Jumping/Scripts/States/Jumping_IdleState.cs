using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_IdleState : State
{
    private JumpingStuff data;
    private bool jumpPressed = false;
    private bool tpPressed = false;

    public override void Enter()
    {
        base.Enter();
        data = stateMachine.variables.Get<JumpingStuff>("Data");
        data.stateText.text = "Idle";
        data.animator.SetTrigger("Idle");
    }

    public override void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            jumpPressed = true;
        if (Input.GetKey(KeyCode.Escape))
            tpPressed = true;
    }

    public override void FixedUpdate()
    {
        if (data.groundCheck.isGrounded && jumpPressed)
            stateMachine.ChangeState(typeof(Jumping_Jumping));
        else if (data.groundCheck.isGrounded == false)
            stateMachine.ChangeState(typeof(Jumping_Falling));
        else if (tpPressed)        
            data.rb.transform.position = new Vector3(0, 5f, 0);        
    }

    public override void Exit()
    {
        base.Exit();
    }
}

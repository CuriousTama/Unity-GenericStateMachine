using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericStateMachine;

public class StackStateB : StackMotherState
{
    RectTransform BTransform;
    float baseYPos;
    float floatingTime = 0f;

    public override void Enter()
    {
        base.Enter();
        BTransform = stateMachine.variables.Get<RectTransform>("B");
        baseYPos = BTransform.position.y;
    }
    public override void Resume()
    {
        base.Resume();
        baseYPos = BTransform.position.y;
        floatingTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();

        BTransform.position = new Vector3(BTransform.position.x, baseYPos, BTransform.position.z);
    }

    public override void Pause()
    {
        base.Pause();

        BTransform.position = new Vector3(BTransform.position.x, baseYPos, BTransform.position.z);
    }

    public override void Update()
    {
        base.Update();

        floatingTime += Time.deltaTime * 17.5f;
        BTransform.position = new Vector3(BTransform.position.x, baseYPos + Mathf.PingPong(floatingTime, 15f), BTransform.position.z);
    }
}

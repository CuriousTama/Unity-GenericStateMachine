using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericStateMachine;

public class StackStateA : StackMotherState
{
    RectTransform ATransform;
    float baseYPos;
    float floatingTime = 0f;

    public override void Enter()
    {
        base.Enter();
        ATransform = stateMachine.variables.Get<RectTransform>("A");
        baseYPos = ATransform.position.y;
    }
    public override void Resume()
    {
        base.Resume();
        baseYPos = ATransform.position.y;
        floatingTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();

        ATransform.position = new Vector3(ATransform.position.x, baseYPos, ATransform.position.z);
    }

    public override void Pause()
    {
        base.Pause();

        ATransform.position = new Vector3(ATransform.position.x, baseYPos, ATransform.position.z);
    }

    public override void Update()
    {
        base.Update();

        floatingTime += Time.deltaTime * 17.5f;
        ATransform.position = new Vector3(ATransform.position.x, baseYPos + Mathf.PingPong(floatingTime, 15f), ATransform.position.z);
    }
}

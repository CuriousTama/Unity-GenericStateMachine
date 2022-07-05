using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericStateMachine;

public class StackStateC : StackMotherState
{
    RectTransform CTransform;
    float baseYPos;
    float floatingTime = 0f;

    public override void Enter()
    {
        base.Enter();
        CTransform = stateMachine.variables.Get<RectTransform>("C");
        baseYPos = CTransform.position.y;
    }
    public override void Resume()
    {
        base.Resume();
        baseYPos = CTransform.position.y;
        floatingTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();

        CTransform.position = new Vector3(CTransform.position.x, baseYPos, CTransform.position.z);
    }

    public override void Pause()
    {
        base.Pause();

        CTransform.position = new Vector3(CTransform.position.x, baseYPos, CTransform.position.z);
    }

    public override void Update()
    {
        base.Update();

        floatingTime += Time.deltaTime * 17.5f;
        CTransform.position = new Vector3(CTransform.position.x, baseYPos + Mathf.PingPong(floatingTime, 15f), CTransform.position.z);
    }
}

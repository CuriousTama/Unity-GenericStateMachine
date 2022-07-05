using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCallbacks : MonoBehaviour
{
    [SerializeField] private GenericStateMachine.StateMachine stateMachine;
    [SerializeField] private StackPrinter printer;

    public void AddStateA()
    {
        stateMachine.AddState(typeof(StackStateA));
        printer.hasChange = true;
    }

    public void AddStateB()
    {
        stateMachine.AddState(typeof(StackStateB));
        printer.hasChange = true;
    }

    public void AddStateC()
    {
        stateMachine.AddState(typeof(StackStateC));
        printer.hasChange = true;
    }

    public void GoBackState()
    {
        stateMachine.RemoveState();
        printer.hasChange = true;
    }

    public void ClearState()
    {
        stateMachine.Clear();
        printer.hasChange = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPrinter : MonoBehaviour
{
    [SerializeField] private GenericStateMachine.StateMachine stateMachine;
    [SerializeField] private Transform instantiateParent;
    [SerializeField] private StackPrintedElement printElement;
    [HideInInspector] public bool hasChange = true;

    private void LateUpdate()
    {
        if (!hasChange)
            return;

        for (int i = 0; i < instantiateParent.childCount; i++)
        {
            Destroy(instantiateParent.GetChild(i).gameObject);
        }

        foreach (string state in stateMachine.GetStatesAsString())
        {
            StackPrintedElement element = Instantiate(printElement, instantiateParent);
            element.SetString(state);
            element.transform.SetAsFirstSibling();
        }

        hasChange = false;
    }
}

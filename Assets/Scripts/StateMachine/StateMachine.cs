using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private List<MonoScript> states;
    [SerializeField] private bool cachingStates = false;

    private IState[] stateCache;
    private IState currentState;

    private void OnValidate()
    {
        states.RemoveAll((state) =>
        {
            bool returnValue = state != null && typeof(IState).IsAssignableFrom(state.GetClass()) == false;

            if (returnValue)
                Debug.LogWarning(state.GetClass().Name + " is not a state (IState interface) and have benn removed.");

            return returnValue;
        });
    }

    private void Awake()
    {
        if (cachingStates)
            CreateCache();

        ChangeState(typeof(TestState));
        ChangeState(typeof(StateMachine));
    }

    private void Update()
    {
        currentState?.PreUpdate(this);
        currentState?.Update(this);
    }

    private void LateUpdate()
    {
        currentState?.LateUpdate(this);
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate(this);
    }



    private void CreateCache()
    {
        states.RemoveAll((state) => state == null || typeof(IState).IsAssignableFrom(state.GetClass()) == false);

        stateCache = new IState[states.Count];

        for (int i = 0; i < states.Count; i++)
        {
            stateCache[i] = System.Activator.CreateInstance(states[i].GetClass()) as IState;
        }
    }



    public void ChangeState(System.Type type)
    {
        if (!typeof(IState).IsAssignableFrom(type))
        {
            Debug.LogWarning(type.Name + " is not a state (IState interface), ChangeState() canceled.");
            return;
        }

        if (cachingStates && stateCache.Where((state) => state.GetType() == type).FirstOrDefault() == null)
        {
            Debug.LogWarning(type.Name + " is not repetoried in the GameObject, ChangeState() canceled.");
            return;
        }



        currentState?.Exit(this);

        if (cachingStates)
            currentState = stateCache.Where((state) => state.GetType() == type).First();
        else
            currentState = System.Activator.CreateInstance(type) as IState;

        currentState?.Enter(this);
    }
}

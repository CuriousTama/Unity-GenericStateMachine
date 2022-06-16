using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class StateMachine : MonoBehaviour
{
    [Space(10f)]
    [Tooltip("The state set on Awake of GameObject. can be null for no starting state")]
    [SerializeField] private MonoScript startingState = null;
    [Tooltip("If true skip the next PreUpdate, Update and LateUpdate of the new state when using ChangeState() Method.")]
    [SerializeField] private bool changingStateSkip = false;
    [Space(10f)]
    [Tooltip("Can't be true at same time as caching")]
    [SerializeField] private bool canHaveAnyStates = true;
    [Tooltip("If true generate every possible states on Awake of GameObject and keep theme in a cache.")]
    [SerializeField] private bool cachingStates = false;
    [Tooltip("Is used when canHaveAnyStates is false")]
    [SerializeField] private List<MonoScript> possibleStates = new List<MonoScript>();


    [SerializeField] private StateVariable[] variables;

    private State[] m_stateCache;
    private State m_currentState;
    private bool m_blockingNextFrame = false;

    private void OnValidate()
    {
        possibleStates.RemoveAll((state) =>
        {
            bool returnValue = state != null && typeof(State).IsAssignableFrom(state.GetClass()) == false;

            if (returnValue)
                Debug.LogWarning(state.GetClass().Name + " is not a state (IState interface) and have benn removed.");

            return returnValue;
        });

        if (cachingStates)
            canHaveAnyStates = false;
    }

    private void Awake()
    {
        if (cachingStates)
            CreateCache();

        if (startingState != null)
            ChangeState(startingState.GetClass());

        m_blockingNextFrame = false;
    }

    private void Update()
    {
        if (m_blockingNextFrame)
            return;

        m_currentState?.PreUpdate();
        m_currentState?.Update();
    }

    private void LateUpdate()
    {
        if (m_blockingNextFrame)
        {
            m_blockingNextFrame = false;
            return;
        }

        m_currentState?.LateUpdate();
    }

    private void FixedUpdate()
    {
        m_currentState?.FixedUpdate();
    }



    private void CreateCache()
    {
        possibleStates.RemoveAll((state) => state == null || typeof(State).IsAssignableFrom(state.GetClass()) == false);

        m_stateCache = new State[possibleStates.Count];

        for (int i = 0; i < possibleStates.Count; i++)
        {
            m_stateCache[i] = System.Activator.CreateInstance(possibleStates[i].GetClass()) as State;
            m_stateCache[i].SetStateMachine(this);
        }
    }



    public void ChangeState(System.Type type)
    {
        if (!typeof(State).IsAssignableFrom(type))
        {
            Debug.LogWarning(type.Name + " is not a state (IState interface), ChangeState() canceled.");
            return;
        }

        if (!canHaveAnyStates && possibleStates.Where((state) => state != null && state.GetClass() == type).FirstOrDefault() == null)
        {
            Debug.LogWarning(type.Name + " is not repetoried in the GameObject, ChangeState() canceled.");
            return;
        }

        // happen only if you change possibleStates list in runtime (can only be changed in inspector)
        if (cachingStates && m_stateCache.Where((state) => state != null && state.GetType() == type).FirstOrDefault() == null)
        {
            Debug.LogWarning(type.Name + " is not found in the cache, ChangeState() canceled.");
            return;
        }



        m_currentState?.Exit();

        if (cachingStates)
            m_currentState = m_stateCache.Where((state) => state.GetType() == type).First();
        else
            m_currentState = System.Activator.CreateInstance(type) as State;

        m_currentState.SetStateMachine(this);
        m_currentState?.Init();
        m_currentState?.Enter();

        if (changingStateSkip)
            m_blockingNextFrame = true;
    }

    public T GetVariable<T>(string name) where T : Object
    {
        foreach (StateVariable variable in variables)
        {
            if (variable.name == name && variable.obj is T)
            {
                return (T)variable.obj;
            }
        }

        Debug.LogWarning("Variable \"" + name + "\" of type \"" + typeof(T).Name + "\" not found");
        return null;
    }
}

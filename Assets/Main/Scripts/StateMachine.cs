using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace GenericStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        private enum Mode
        {
            Single,
            Stackable
        }

        #region variables
        [Space(10f)]
        [Tooltip("Reference to the set of variables to use.")]
        public StateVariables variables;

        [Space(20f)]
        [Tooltip("Single mode can have only one state.\n" +
            "Stackable mode can have a buffer and return to previous states. (name functions here)")]
        [SerializeField] private Mode stateMachineType = Mode.Single;
        [Tooltip("If true skip the next PreUpdate, Update and LateUpdate of the new state when using ChangeState() Method.")]
        [SerializeField] private bool changingStateSkip = false;
        [Tooltip("Can't be true at same time as caching")]
        [SerializeField] private bool canHaveAnyStates = true;
        [Tooltip("If true generate every possible states on Awake of GameObject and keep theme in a cache.")]
        [SerializeField] private bool cachingStates = false;
        [Space(20f)]

        // MonoScript is an Editor type so not compiled in build.
#if UNITY_EDITOR
        [Tooltip("The state set on Awake of GameObject. can be null for no starting state")]
        [SerializeField] private MonoScript startingState = null;
        [Tooltip("Is used when canHaveAnyStates is false")]
        [SerializeField] private List<MonoScript> possibleStates = new List<MonoScript>();
#endif

        private State[] m_stateCache;
        private State m_currentState;
        private List<State> m_States = new List<State>();
        private bool m_blockingNextFrame = false;

        // SerializeField attribute is here to avoid variable to reset when setting playmode, build or editor reload 
        // and we don't want to see it to HideInInspector attribute.
        [SerializeField, HideInInspector] private bool m_onCreate = true;

        // Need to get AssemblyQualifiedName of types and save them beacause we don't have acces to editor stuff in build (bye bye MonoScript)
        // and System.Type is not Serialized so don't save with "[SerializeField, HideInInspector]" attributes.
        // Find System.Type by using System.Type.GetType("AssemblyQualifiedName") method.
        // Performances is not perfect but still good (I think)
        // sources of benchmark  (https://stackoverflow.com/questions/353342/performance-of-object-gettype/353435#353435).
        [SerializeField, HideInInspector] private List<string> m_possibleStates = new List<string>();
        [SerializeField, HideInInspector] private string m_startingState;
        #endregion


        #region private Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Delay OnValidate() call because of AddComponent throwing a warning if done at a wrong timing.
            EditorApplication.delayCall += _OnValidate;
        }

        private void _OnValidate()
        {
            // Remove OnValidate() call once called.
            EditorApplication.delayCall -= _OnValidate;
            if (this == null) return;

            // Add StateVariables component on adding this component creation.
            // (we don't use [RequireComponent] if the user want to use a StateVariables of another GameObject).
            if (m_onCreate)
            {
                AwakeEditor();
                m_onCreate = false;
            }

            // Reset to null values that are not derived from "State" class.
            possibleStates.ForEach((state) =>
            {
                if (state != null && typeof(State).IsAssignableFrom(state.GetClass()) == false)
                {
                    state = null;
                    Debug.LogWarning(state.GetClass().Name + " is not a state (IState interface) and have been removed.");
                }
            });

            m_possibleStates = possibleStates.Select((state) => state != null ? state.GetClass().AssemblyQualifiedName : "").ToList();
            m_startingState = startingState == null ? null : startingState.GetClass().AssemblyQualifiedName;

            // Can't have cachingStates and canHaveAnyStates variables to true at the same time.
            if (cachingStates)
                canHaveAnyStates = false;
        }
#endif

        private void Awake()
        {
            // Caching states if set to true.
            if (cachingStates)
                CreateCache();

            // Launch the starting state.
            if (string.IsNullOrEmpty(m_startingState) == false)
                ChangeState(Type.GetType(m_startingState));

            // Don't want to block the first state even if the changingStateSkip is true.
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

        private void AwakeEditor()
        {
            StateVariables variables = gameObject.AddComponent<StateVariables>();
            this.variables = variables;
        }

        private void CreateCache()
        {
            // Get every states (redo a check "in case").
            List<string> statesToCreate = m_possibleStates.Where((state) => state != null && typeof(State).IsAssignableFrom(Type.GetType(state))).ToList();

            m_stateCache = new State[statesToCreate.Count];

            // Create states.
            for (int i = 0; i < statesToCreate.Count; i++)
            {
                m_stateCache[i] = System.Activator.CreateInstance(Type.GetType(statesToCreate[i])) as State;
                m_stateCache[i].SetStateMachine(this);
                m_stateCache[i].Init();

            }
        }

        private bool CheckType(Type type)
        {
            // Check if type is derived from "State" class.
            if (!typeof(State).IsAssignableFrom(type))
            {
                Debug.LogWarning(type.Name + " is not a state (IState interface), ChangeState() canceled.");
                return false;
            }

            // Check if the state is in the list of possible states (if canHaveAnyStates is false).
            if (!canHaveAnyStates && m_possibleStates.Where((state) => state != null && state == type.AssemblyQualifiedName).FirstOrDefault() == null)
            {
                Debug.LogWarning(type.Name + " is not repetoried in the GameObject, ChangeState() canceled.");
                return false;
            }

            // Check if the state is in the list of cached states (if canHaveAnyStates is false).
            // Happen only if you change possibleStates list in runtime (can only be changed in inspector).
            if (cachingStates && m_stateCache.Where((state) => state != null && state.GetType() == type).FirstOrDefault() == null)
            {
                Debug.LogWarning(type.Name + " is not found in the cache, ChangeState() canceled.");
                return false;
            }

            return true;
        }

        private bool CheckType<T>()
        {
            // Check if type is derived from "State" class.
            if (!typeof(State).IsAssignableFrom(typeof(T)))
            {
                Debug.LogWarning(typeof(T).Name + " is not a state (IState interface), ChangeState() canceled.");
                return false;
            }

            // Check if the state is in the list of possible states (if canHaveAnyStates is false).
            if (!canHaveAnyStates && m_possibleStates.Where((state) => state != null && state == typeof(T).AssemblyQualifiedName).FirstOrDefault() == null)
            {
                Debug.LogWarning(typeof(T).Name + " is not repetoried in the GameObject, ChangeState() canceled.");
                return false;
            }

            // Check if the state is in the list of cached states (if canHaveAnyStates is false).
            // Happen only if you change possibleStates list in runtime (can only be changed in inspector).
            if (cachingStates && m_stateCache.Where((state) => state != null && state.GetType() == typeof(T)).FirstOrDefault() == null)
            {
                Debug.LogWarning(typeof(T).Name + " is not found in the cache, ChangeState() canceled.");
                return false;
            }

            return true;
        }
        #endregion


        #region Public Methods
        #region ChangeState
        /// <summary>
        /// <para> Single mode : Change the current state. </para>
        /// <para> Stackable mode : Same as AddState(type). </para>
        /// </summary>
        /// <param name="type">The type of the state you want.</param>
        /// <returns>The new state.</returns>
        public State ChangeState(Type type)
        {
            if (stateMachineType == Mode.Stackable)
                return AddState(type);

            if (CheckType(type) == false)
                return null;

            m_currentState?.Exit();

            // Get the cached state or create state.
            if (cachingStates)
                m_currentState = m_stateCache.Where((state) => state.GetType() == type).First();
            else
                m_currentState = System.Activator.CreateInstance(type) as State;


            if (!cachingStates)
            {
                m_currentState?.SetStateMachine(this);
                m_currentState?.Init();
            }

            m_currentState?.Enter();

            if (changingStateSkip)
                m_blockingNextFrame = true;

            return m_currentState;
        }

        /// <summary>
        /// <para> Single mode : Change the current state. </para>
        /// <para> Stackable mode : Same as AddState(type). </para>
        /// </summary>
        /// <typeparam name="T">The type of the state you want.</typeparam>
        /// <returns>The new state.</returns>
        public State ChangeState<T>() where T : State, new()
        {
            if (stateMachineType == Mode.Stackable)
                return AddState<T>();

            if (CheckType<T>() == false)
                return null;

            m_currentState?.Exit();

            // Get the cached state or create state.
            if (cachingStates)
                m_currentState = m_stateCache.Where((state) => state.GetType() == typeof(T)).First();
            else
                m_currentState = new T();


            if (!cachingStates)
            {
                m_currentState?.SetStateMachine(this);
                m_currentState?.Init();
            }

            m_currentState?.Enter();

            if (changingStateSkip)
                m_blockingNextFrame = true;

            return m_currentState;
        }
        #endregion

        #region AddState
        /// <summary>
        /// <para> Single mode : Same as ChangeState(type). </para>
        /// <para> Stackable mode : Set the current state to pause and add a new one of "type". </para>
        /// If cache activated resume if it already is in the previous states.
        /// </summary>
        /// <param name="type">The type of the state you want to add.</param>
        /// <returns>The new state.</returns>
        public State AddState(Type type)
        {
            if (stateMachineType == Mode.Single)
                return ChangeState(type);

            if (CheckType(type) == false)
                return null;


            State newState;
            if (cachingStates)
                newState = m_stateCache.Where((state) => state.GetType() == type).First();
            else
                newState = System.Activator.CreateInstance(type) as State;

            m_currentState?.Pause();

            m_States.Add(newState);
            m_currentState = newState;

            if (cachingStates)
            {
                if (m_States.Where((state) => state == m_currentState).Count() > 1)
                    m_currentState?.Resume();
                else
                    m_currentState?.Enter();
            }
            else
            {
                m_currentState?.SetStateMachine(this);
                m_currentState?.Init();
                m_currentState?.Enter();
            }

            return m_currentState;
        }

        /// <summary>
        /// <para> Single mode : Same as ChangeState(type). </para>
        /// <para> Stackable mode : Set the current state to pause and add a new one of "type". </para>
        /// If cache activated resume if it already is in the previous states.
        /// </summary>
        /// <typeparam name="T">The type of the state you want to add.</typeparam>
        /// <returns>The new state.</returns>
        public State AddState<T>() where T : State, new()
        {
            if (stateMachineType == Mode.Single)
                return ChangeState<T>();

            if (CheckType<T>() == false)
                return null;


            State newState;
            if (cachingStates)
                newState = m_stateCache.Where((state) => state.GetType() == typeof(T)).First();
            else
                newState = new T();

            m_currentState?.Pause();

            m_States.Add(newState);
            m_currentState = newState;

            if (cachingStates)
            {
                if (m_States.Where((state) => state == m_currentState).Count() > 1)
                    m_currentState?.Resume();
                else
                    m_currentState?.Enter();
            }
            else
            {
                m_currentState?.SetStateMachine(this);
                m_currentState?.Init();
                m_currentState?.Enter();
            }

            return m_currentState;
        }
        #endregion

        /// <summary>
        /// <para> Single mode : Set the state to null. </para>
        /// <para> Stackable mode : Remove the current state and resume the previous one. </para>
        /// </summary>
        /// <returns>The new running state.</returns>
        public State RemoveState()
        {
            if (stateMachineType == Mode.Single)
            {
                m_currentState?.Exit();
                m_currentState = null;
                return m_currentState;
            }

            if (cachingStates)
            {
                if (m_States.Where((state) => state == m_currentState).Count() > 1)
                    m_currentState?.Pause();
                else
                    m_currentState?.Exit();
            }
            else
                m_currentState?.Exit();

            if (m_States.Count() > 0)
                m_States.RemoveAt(m_States.Count - 1);

            m_currentState = m_States.LastOrDefault();

            m_currentState?.Resume();

            return m_currentState;
        }

        /// <summary>
        /// <para> Single mode : Set the state to null. </para>
        /// <para> Stackable mode : Remove all states and set the state to null. </para>
        /// </summary>
        public void Clear()
        {
            if (stateMachineType == Mode.Single)
                m_currentState?.Exit();
            else if (stateMachineType == Mode.Stackable)
            {
                for (int i = m_States.Count - 1; i >= 0; i--)
                    m_States[i]?.Exit();

                m_States.Clear();
            }

            m_currentState = null;
        }

        /// <summary>
        /// Pause the current running state.
        /// </summary>
        /// <returns>If the state was playing and well paused.</returns>
        public bool Pause()
        {
            if (m_currentState != null && m_currentState.status == StateStatus.Playing)
            {
                m_currentState?.Pause();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resume the current running state.
        /// </summary>
        /// <returns>If the state was paused and well resumed.</returns>
        public bool Resume()
        {
            if (m_currentState != null && m_currentState.status == StateStatus.Paused)
            {
                m_currentState?.Resume();
                return true;
            }

            return false;
        }


        /// <summary>
        /// Get total number of states.
        /// </summary>
        /// <returns>
        /// <para> Single mode : If no current state 0 else 1. </para>
        /// <para> Stackable mode : number of states. </para>
        /// </returns>
        public int GetStatesCount()
        {
            if (stateMachineType == Mode.Single)
                return (m_currentState == null) ? 0 : 1;

            if (stateMachineType == Mode.Stackable)
                return m_States.Count;

            return 0;
        }

        /// <summary>
        /// Get states list.
        /// </summary>
        /// <returns>
        /// A list with all state as string (in order).
        /// </returns>
        public List<string> GetStatesAsString()
        {
            List<string> list = new List<string>();

            if (stateMachineType == Mode.Single)
            {
                if (m_currentState != null)
                    list.Add(m_currentState.GetType().Name);
            }
            else if (stateMachineType == Mode.Stackable)
            {
                foreach (State state in m_States)
                {
                    list.Add(state.GetType().Name);
                }
            }

            return list;
        }

        /// <summary>
        /// Get state list.
        /// </summary>
        /// <returns>
        /// A list with all states as System.Type (in order).
        /// </returns>
        public List<Type> GetStatesAsType()
        {
            List<Type> list = new List<Type>();

            if (stateMachineType == Mode.Single)
            {
                if (m_currentState != null)
                    list.Add(m_currentState.GetType());
            }
            else if (stateMachineType == Mode.Stackable)
            {
                foreach (State state in m_States)
                {
                    list.Add(state.GetType());
                }
            }

            return list;
        }

        /// <summary>
        /// Get the current running state.
        /// </summary>
        /// <returns>
        /// The current running state.
        /// </returns>
        public State GetCurrentState()
        {
            return m_currentState;
        }
        #endregion
    }
}

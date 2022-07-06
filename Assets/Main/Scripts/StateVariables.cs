using System.Collections.Generic;
using UnityEngine;

namespace GenericStateMachine
{
    public class StateVariables : MonoBehaviour
    {
        public List<StateVariable> variables;

        /// <summary>
        /// Get variable of type T
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="name">Name of the wanted value</param>
        /// <returns>The value casted as T if possible, return null if name not found or not the castable</returns>
        public T Get<T>(string name) where T : Object
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

    [System.Serializable]
    public struct StateVariable
    {
        public string name;
        public Object obj;
    }
}
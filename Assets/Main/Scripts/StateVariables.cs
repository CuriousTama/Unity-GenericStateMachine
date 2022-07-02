using System.Collections.Generic;
using UnityEngine;

public class StateVariables : MonoBehaviour
{
    public List<StateVariable> variables;

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

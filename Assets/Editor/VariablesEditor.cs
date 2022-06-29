using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(StateVariables))]
public class VariablesEditor : Editor
{
    private bool variablesFoldout = true;

    SerializedProperty variables;
    ReorderableList list;
    private void OnEnable()
    {
        variables = serializedObject.FindProperty("variables");
        list = new ReorderableList(serializedObject, variables, true, true, true, true);

        list.drawHeaderCallback = DrawHeader;
        list.drawElementCallback = DrawListItems;
    }

    void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Variables");
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); // The element in the list

        float borderPadding = 50f;
        float totalWidth = EditorGUIUtility.currentViewWidth;
        totalWidth -= 50f; // name label width
        totalWidth -= 85; // Value label width + spacing
        totalWidth -= borderPadding;

        float nameFieldWidth = totalWidth * 0.3f;
        totalWidth -= nameFieldWidth;
        float elementWidth = totalWidth;

        nameFieldWidth = Mathf.Max(EditorGUIUtility.singleLineHeight, nameFieldWidth);
        elementWidth = Mathf.Max(EditorGUIUtility.singleLineHeight, elementWidth);

        float posX = rect.x;
        EditorGUI.LabelField(new Rect(posX, rect.y, 50f, EditorGUIUtility.singleLineHeight), "Name");
        posX += 50f;
        EditorGUI.PropertyField(new Rect(posX, rect.y, nameFieldWidth, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("name"),
            GUIContent.none
        );
        posX += nameFieldWidth;
        posX += 30f; // spacing

        EditorGUI.LabelField(new Rect(posX, rect.y, 50f, EditorGUIUtility.singleLineHeight), "Value");
        posX += 50f;
        EditorGUI.PropertyField(new Rect(posX, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("obj"),
            GUIContent.none
        );

    }


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

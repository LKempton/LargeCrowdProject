﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrowdAI.CrowdController)), CanEditMultipleObjects]
public class CrowdEditorScript : Editor {

    public SerializedProperty
        crowdFormation_Prop,
        rows_Prop,
        columns_Prop,
        minOffset_Prop,
        maxOffset_Prop,
        tiltAmount_Prop,
        startHeight_Prop,
        crowdObject_Prop,
        groupNames_Prop,
        crowdStates_Prop;
    

    void OnEnable()
    {
        crowdFormation_Prop = serializedObject.FindProperty("_crowdFormation");
        rows_Prop = serializedObject.FindProperty("_rows");
        columns_Prop = serializedObject.FindProperty("_columns");
        minOffset_Prop = serializedObject.FindProperty("_minOffset");
        maxOffset_Prop = serializedObject.FindProperty("_maxOffset");
        tiltAmount_Prop = serializedObject.FindProperty("_tiltAmount");
        startHeight_Prop = serializedObject.FindProperty("_startHeight");
        crowdObject_Prop = serializedObject.FindProperty("_crowdObject");
        groupNames_Prop = serializedObject.FindProperty("_groupNames");
        crowdStates_Prop = serializedObject.FindProperty("_crowdStates");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(crowdFormation_Prop);

        CrowdAI.CrowdFormation cF = (CrowdAI.CrowdFormation)crowdFormation_Prop.enumValueIndex;

        // Commented stuff is not yet implemented.
        switch (cF)
        {
            case CrowdAI.CrowdFormation.SQUARE:
                EditorGUILayout.ObjectField(crowdObject_Prop, new GUIContent("Crowd Object"));
                EditorGUILayout.PropertyField(rows_Prop, new GUIContent("Rows"));
                EditorGUILayout.PropertyField(columns_Prop, new GUIContent("Columns"));
                EditorGUILayout.PropertyField(minOffset_Prop, new GUIContent("Minimum Offset"));
                EditorGUILayout.PropertyField(maxOffset_Prop, new GUIContent("Maximum Offset"));
                EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("Tilt Amount"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("Height Offset"));
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);
                break;
            case CrowdAI.CrowdFormation.CIRCLE:
                EditorGUILayout.ObjectField(crowdObject_Prop, new GUIContent("Crowd Object"));
                EditorGUILayout.PropertyField(rows_Prop, new GUIContent("Layers"));
                //EditorGUILayout.PropertyField(minOffset_Prop, new GUIContent("Minimum Offset"));
                //EditorGUILayout.PropertyField(maxOffset_Prop, new GUIContent("Maximum Offset"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("Height Offset"));
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);
                break;
            case CrowdAI.CrowdFormation.RING:
                //EditorGUILayout.ObjectField(crowdObject_Prop, new GUIContent("Crowd Object"));
                //EditorGUILayout.PropertyField(rows_Prop, new GUIContent("Layers"));
                //EditorGUILayout.PropertyField(minOffset_Prop, new GUIContent("Minimum Offset"));
                //EditorGUILayout.PropertyField(maxOffset_Prop, new GUIContent("Maximum Offset"));
                //EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("Height Offset"));
                //GUIArray(groupNames_Prop);
                //GUIArray(crowdStates_Prop);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void GUIArray(SerializedProperty val)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(val, true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

}
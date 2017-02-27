using UnityEngine;
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
        startHeight_Prop;
    

    void OnEnable()
    {
        crowdFormation_Prop = serializedObject.FindProperty("_crowdFormation");
        rows_Prop = serializedObject.FindProperty("_rows");
        columns_Prop = serializedObject.FindProperty("_columns");
        minOffset_Prop = serializedObject.FindProperty("_minOffset");
        maxOffset_Prop = serializedObject.FindProperty("_maxOffset");
        tiltAmount_Prop = serializedObject.FindProperty("_tiltAmount");
        startHeight_Prop = serializedObject.FindProperty("_startHeight");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(crowdFormation_Prop);

        CrowdAI.CrowdFormation cF = (CrowdAI.CrowdFormation)crowdFormation_Prop.enumValueIndex;

        switch (cF)
        {
            case CrowdAI.CrowdFormation.SQUARE:
                EditorGUILayout.PropertyField(rows_Prop, new GUIContent("_rows"));
                EditorGUILayout.PropertyField(columns_Prop, new GUIContent("_columns"));
                EditorGUILayout.PropertyField(minOffset_Prop, new GUIContent("_minOffset"));
                EditorGUILayout.PropertyField(maxOffset_Prop, new GUIContent("_maxOffset"));
                EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("_tiltAmount"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("_startHeight"));
                break;
            case CrowdAI.CrowdFormation.CIRCLE:
                EditorGUILayout.PropertyField(rows_Prop, new GUIContent("_rows"));
                EditorGUILayout.PropertyField(minOffset_Prop, new GUIContent("_minOffset"));
                EditorGUILayout.PropertyField(maxOffset_Prop, new GUIContent("_maxOffset"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("_startHeight"));
                break;
            case CrowdAI.CrowdFormation.RING:
                EditorGUILayout.PropertyField(rows_Prop, new GUIContent("_rows"));
                EditorGUILayout.PropertyField(minOffset_Prop, new GUIContent("_minOffset"));
                EditorGUILayout.PropertyField(maxOffset_Prop, new GUIContent("_maxOffset"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("_startHeight"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

}

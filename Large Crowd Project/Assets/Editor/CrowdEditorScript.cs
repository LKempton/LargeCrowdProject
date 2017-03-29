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
        startHeight_Prop,
        crowdObject_Prop,
        groupNames_Prop,
        randomGroupDistribution_Prop,
        crowdDensity_Prop,
        innerRadius_Prop,
        crowdStates_Prop;
    

    void OnEnable()
    {
        crowdFormation_Prop = serializedObject.FindProperty("_crowdFormation");
        innerRadius_Prop = serializedObject.FindProperty("_innerRadius");
        crowdDensity_Prop = serializedObject.FindProperty("_density");
        tiltAmount_Prop = serializedObject.FindProperty("_tiltAmount");
        startHeight_Prop = serializedObject.FindProperty("_startHeight");
        crowdObject_Prop = serializedObject.FindProperty("_placeholderPrefab");
        groupNames_Prop = serializedObject.FindProperty("_groupNames");
        crowdStates_Prop = serializedObject.FindProperty("_crowdStates");
        randomGroupDistribution_Prop = serializedObject.FindProperty("_randomGroupDist");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(crowdFormation_Prop);

        CrowdAI.CrowdFormation cF = (CrowdAI.CrowdFormation)crowdFormation_Prop.enumValueIndex;

        CrowdAI.CrowdController script = (CrowdAI.CrowdController)target;

        // Commented stuff is not yet implemented.
        switch (cF)
        {
            case CrowdAI.CrowdFormation.SQUARE:
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                EditorGUILayout.PropertyField(randomGroupDistribution_Prop, new GUIContent("Grouped Randomly?"));
                EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("Tilt Amount"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("Height Offset"));
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);
                
                if (GUILayout.Button("Generate Crowd"))
                {
                    script.GenerateCrowd();
                }
                break;
            case CrowdAI.CrowdFormation.CIRCLE:
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                EditorGUILayout.PropertyField(randomGroupDistribution_Prop, new GUIContent("Grouped Randomly?"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("Height Offset"));
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);

                if (GUILayout.Button("Generate Crowd"))
                {
                    script.GenerateCrowd();
                }
                break;
            case CrowdAI.CrowdFormation.RING:
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                EditorGUILayout.PropertyField(randomGroupDistribution_Prop, new GUIContent("Grouped Randomly?"));
                EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("Tilt Amount"));
                EditorGUILayout.PropertyField(startHeight_Prop, new GUIContent("Height Offset"));
                EditorGUILayout.PropertyField(innerRadius_Prop, new GUIContent("Hole Radius"));
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);


                if (GUILayout.Button("Generate Crowd"))
                {
                    script.GenerateCrowd();
                }
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

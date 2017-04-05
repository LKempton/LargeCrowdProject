using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrowdAI.CrowdController)), CanEditMultipleObjects]
public class CrowdEditorScript : Editor {

    private EditorSquareScript editorScript;
    private EditorSquareScript childScript;
    CrowdAI.CrowdController script;
    private int _estimatedCount = 0;

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
        script = (CrowdAI.CrowdController)target;
    }

    public override void OnInspectorGUI()
    {
        _estimatedCount = script.GetPrediction();
        serializedObject.Update();

        EditorGUILayout.PropertyField(crowdFormation_Prop);

        CrowdAI.CrowdFormation cF = (CrowdAI.CrowdFormation)crowdFormation_Prop.enumValueIndex;

        EditorGUILayout.LabelField("Approx Crowd: ", _estimatedCount.ToString());

        editorScript = script.gameObject.GetComponent<EditorSquareScript>();

        childScript = script.gameObject.GetComponentsInChildren<EditorSquareScript>()[1];

        string descriptionText = "\nEdit the settings above to change how the crowd spawns, then press 'Generate Crowd' to create a new 'Crowd Source' object.\nYou can move 'Crowd Source around like any other model in Unity.";

        EditorStyles.label.wordWrap = true;

        // Commented stuff is not yet implemented.
        switch (cF)
        {
            case CrowdAI.CrowdFormation.SQUARE:
                editorScript.isCircle = false;
                childScript.isCircle = false;
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                EditorGUILayout.PropertyField(randomGroupDistribution_Prop, new GUIContent("Grouped Randomly?"));
                //EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("Tilt Amount"));

                if (GUILayout.Button("Generate Crowd" ,GUILayout.Width(200), GUILayout.Height(25)))
                {
                    script.GenerateCrowd();
                }
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);
            
                EditorGUILayout.LabelField(descriptionText);

                break;
            case CrowdAI.CrowdFormation.CIRCLE:
                editorScript.isCircle = true;
                childScript.isCircle = true;
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                EditorGUILayout.PropertyField(randomGroupDistribution_Prop, new GUIContent("Grouped Randomly?"));
                if (GUILayout.Button("Generate Crowd", GUILayout.Width(200), GUILayout.Height(25)))
                {
                    script.GenerateCrowd();
                }
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);

                EditorGUILayout.LabelField(descriptionText);

                break;
            case CrowdAI.CrowdFormation.RING:
                editorScript.isCircle = true;
                childScript.isCircle = true;
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                EditorGUILayout.PropertyField(randomGroupDistribution_Prop, new GUIContent("Grouped Randomly?"));
                //EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("Tilt Amount"));
                EditorGUILayout.PropertyField(innerRadius_Prop, new GUIContent("Hole Radius"));
                if (GUILayout.Button("Generate Crowd", GUILayout.Width(200), GUILayout.Height(25)))
                {
                    script.GenerateCrowd();
                }
                GUIArray(groupNames_Prop);
                GUIArray(crowdStates_Prop);

                EditorGUILayout.LabelField(descriptionText);

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

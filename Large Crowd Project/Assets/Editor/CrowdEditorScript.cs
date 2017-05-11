using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrowdAI.CrowdController)), CanEditMultipleObjects]
public class CrowdEditorScript : Editor {

    private EditorSquareScript editorScript;
    private EditorSquareScript childScript;
    CrowdAI.CrowdController script;
    bool _showDebug = false;

    public SerializedProperty
        crowdFormation_Prop,
        rows_Prop,
        columns_Prop,
        minOffset_Prop,
        maxOffset_Prop,
        tiltAmount_Prop,
        startHeight_Prop,
        crowdObject_Prop,
        crowdDensity_Prop,
        rotation_Prop,
        innerRadius_Prop,
        models_Prop,
        modelAmounts_prop,
        crowdStates_Prop;
    

    void OnEnable()
    { 
        crowdFormation_Prop = serializedObject.FindProperty("_crowdFormation");
        innerRadius_Prop = serializedObject.FindProperty("_innerRadius");
        rotation_Prop = serializedObject.FindProperty("_rotDir");
        crowdDensity_Prop = serializedObject.FindProperty("_density");
        tiltAmount_Prop = serializedObject.FindProperty("_tiltAmount");
        startHeight_Prop = serializedObject.FindProperty("_startHeight");
        crowdObject_Prop = serializedObject.FindProperty("_placeholderPrefab");
        crowdStates_Prop = serializedObject.FindProperty("_crowdStates");
        models_Prop = serializedObject.FindProperty("_groupModels");


        script = (CrowdAI.CrowdController)target;
        
    }

    public override void OnInspectorGUI()
    {
       int _estimatedCount = script.GetPrediction();
        int _currentTotal = script.Size;

        

        serializedObject.Update();

        EditorGUILayout.LabelField("Crowd Size: ", _currentTotal.ToString());

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
                EditorGUILayout.Slider(rotation_Prop, 0, 360, new GUIContent("Rotation"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
             
                //EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("Tilt Amount"));
                break;


            case CrowdAI.CrowdFormation.CIRCLE:
                editorScript.isCircle = true;
                childScript.isCircle = true;
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.Slider(rotation_Prop, 0, 360, new GUIContent("Rotation"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                break;


            case CrowdAI.CrowdFormation.RING:
                editorScript.isCircle = true;
                childScript.isCircle = true;
                EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                EditorGUILayout.Slider(rotation_Prop, 0, 360, new GUIContent("Rotation"));
                EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
               
                //EditorGUILayout.PropertyField(tiltAmount_Prop, new GUIContent("Tilt Amount"));
                EditorGUILayout.PropertyField(innerRadius_Prop, new GUIContent("Hole Radius"));

                break;
        }

        if (GUILayout.Button("Generate Crowd", GUILayout.Width(200), GUILayout.Height(25)))
        {
            script.GenerateCrowd();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUIArray(crowdStates_Prop);

        EditorGUILayout.LabelField(descriptionText);

        if (GUILayout.Button("Manage Models & Groups", GUILayout.Width(200), GUILayout.Height(25)))
        {
            var window = (CrowdAI.GroupUI)CrowdAI.GroupUI.ShowWindow();
            window.Controller = script;
        }

        _showDebug = EditorGUILayout.Toggle("Show Debug Options", _showDebug);

        if (_showDebug)
        {
            if (GUILayout.Button("Log Debug Information", GUILayout.Width(200), GUILayout.Height(25)))
            {
                script.ShowDebugInfo();
            }

            if (GUILayout.Button("Save Controller Data", GUILayout.Width(200), GUILayout.Height(25)))
            {
                script.SaveAll(false);
                Debug.Log("Saved Controller Data");

            }
            /*
            if (GUILayout.Button("LoadController Data"))
            {
                script.ReadAll();
                Debug.Log("Loaded Controller Data");

            }
            */
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

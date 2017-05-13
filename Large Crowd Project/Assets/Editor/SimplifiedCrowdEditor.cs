using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdAI
{
    [CustomEditor(typeof(SimplifiedCrowdController)), CanEditMultipleObjects]
    public class SimplifiedCrowdEditor : Editor
    {
        private SimplifiedCrowdController script;
        private EditorSquareScript editorScript;
        private EditorSquareScript childScript;

        public SerializedProperty
            crowdFormation_Prop,
            team_Prop,
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
            crowdStates_Prop;


        void OnEnable()
        {
            crowdFormation_Prop = serializedObject.FindProperty("_crowdFormation");
            team_Prop = serializedObject.FindProperty("_team");
            innerRadius_Prop = serializedObject.FindProperty("_innerRadius");
            rotation_Prop = serializedObject.FindProperty("_rotDir");
            crowdDensity_Prop = serializedObject.FindProperty("_density");
            tiltAmount_Prop = serializedObject.FindProperty("_tiltAmount");
            startHeight_Prop = serializedObject.FindProperty("_startHeight");
            crowdObject_Prop = serializedObject.FindProperty("_startingPrefab");
            models_Prop = serializedObject.FindProperty("_groupModels");


            script = (SimplifiedCrowdController)target;

        }

        public override void OnInspectorGUI()
        {
            editorScript = script.gameObject.GetComponent<EditorSquareScript>();
            childScript = script.gameObject.GetComponentsInChildren<EditorSquareScript>()[1];

            int _estimatedCount = script.GetPrediction();
            int _currentTotal = script.CrowdCount;

            serializedObject.Update();

            EditorGUILayout.LabelField("Crowd Size: ", _currentTotal.ToString());

            EditorGUILayout.PropertyField(crowdFormation_Prop);

            EditorGUILayout.PropertyField(team_Prop);

            CrowdFormation cF = (CrowdFormation)crowdFormation_Prop.enumValueIndex;

            EditorGUILayout.LabelField("Approx Crowd: ", _estimatedCount.ToString());

            string descriptionText = "\nEdit the settings above to change how the crowd spawns, then press 'Generate Crowd' to create a new 'Crowd Source' object.\nYou can move 'Crowd Source around like any other model in Unity.";

            EditorStyles.label.wordWrap = true;
            
            switch (cF)
            {
                case CrowdFormation.SQUARE:
                    editorScript.isCircle = false;
                    childScript.isCircle = false;
                    EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                    EditorGUILayout.Slider(rotation_Prop, 0, 360, new GUIContent("Rotation"));
                    EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                    break;


                case CrowdFormation.CIRCLE:
                    editorScript.isCircle = true;
                    childScript.isCircle = true;
                    EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                    EditorGUILayout.Slider(rotation_Prop, 0, 360, new GUIContent("Rotation"));
                    EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                    break;


                case CrowdFormation.RING:
                    editorScript.isCircle = true;
                    childScript.isCircle = true;
                    EditorGUILayout.Slider(crowdDensity_Prop, 0, 1, new GUIContent("Crowd Density"));
                    EditorGUILayout.Slider(rotation_Prop, 0, 360, new GUIContent("Rotation"));
                    EditorGUILayout.PropertyField(crowdObject_Prop, new GUIContent("Crowd Placeholder"));
                    EditorGUILayout.PropertyField(innerRadius_Prop, new GUIContent("Hole Radius"));

                    break;
            }

            if (GUILayout.Button("Generate Crowd", GUILayout.Width(200), GUILayout.Height(25)))
            {
                script.GenerateCrowd(script.transform.GetChild(0).transform.localPosition);
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUIArray(crowdStates_Prop);

            EditorGUILayout.LabelField(descriptionText);

            serializedObject.ApplyModifiedProperties();
        }

        void GUIArray(SerializedProperty val)
        {
            if (val == null)
            {

                return;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(val, true);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

    }
}

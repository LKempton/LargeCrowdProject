using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CrowdAI
{
    /// <summary>
    /// Editor GUI for Handling groups
    /// </summary>
    
    public class GroupUI : EditorWindow
    {
        private CrowdController _crowdController;
        private CrowdGroup[] _currentGroups;
        private CrowdGroup _unassignedGroup;

        private GUIStyle _genericTextStyle;
        private GUIStyle _headingTextStyle;
        private string _newGroupName = "Group Name";

        private int _numberOfModels = 0;

        GameObject[] levelsOfDetail = new GameObject[30];

        Vector2 scrollPosition = new Vector2();

        void Awake()
        {
            _genericTextStyle = new GUIStyle();
            _headingTextStyle = new GUIStyle();

            _headingTextStyle.fontSize = 20;
            _headingTextStyle.padding = new RectOffset(8, 8, 8, 8);

            _headingTextStyle.fontStyle = FontStyle.Bold;

            _genericTextStyle.fontSize = 10;
            _genericTextStyle.padding = new RectOffset(6, 3, 6, 6);
        }


        void OnGUI()
        {
            if (_crowdController != null)
            {
                _currentGroups = _crowdController.GetGroups();
                _unassignedGroup = _crowdController.GetUnassignedGroup;

                
                GUILayout.Label("Add New Group",_headingTextStyle);
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Group Name:", _genericTextStyle, GUILayout.Width(120));

                _newGroupName = GUILayout.TextField(_newGroupName, 25, GUILayout.Width(200));
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Add New Group", GUILayout.Width(200)))
                {

                    if (_newGroupName != "Group Name")
                    {
                        Debug.Log("Tried to talk to controller");
                        _crowdController.AddGroup(_newGroupName);
                    }
                 
                }
                


                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                if (_unassignedGroup != null)
                {
                    ShowGroup(_unassignedGroup);
                }
                
                if (_currentGroups != null)
                {
                    for (int i = 0; i < _currentGroups.Length; i++)
                    {
                        ShowGroup(_currentGroups[i]);
                    }
                }

                GUILayout.EndScrollView();
                
            }
        }

        [MenuItem("Window/GroupUI")]
        public static EditorWindow ShowWindow()
        {
          EditorWindow window =  EditorWindow.GetWindow(typeof(GroupUI));

            window.Show();

            return window;
        }

            public CrowdController Controller
        {
            set
            {
                if (value != null)
                {
                    _crowdController = value;
                }
            }
        }
        

        public void ShowGroup(CrowdGroup group)
        {
            GUILayout.Label("Group Name: ");

            group.GroupName = GUILayout.TextField(group.GroupName);

            GUILayout.BeginHorizontal();

            GUILayout.Label("Number of Models: ");
            _numberOfModels = EditorGUILayout.IntField(_numberOfModels, GUILayout.Width(100));
            GUILayout.EndHorizontal();

            for (int i = 0; i < _numberOfModels; i++)
            {
                GUILayout.Label("Crowd Character " + i);
                
                //characters[characters.Count - 1], typeof(GameObject), true, GUILayout.Width(200))
                //for (int j = 0; j < _crowdController._LODCount; j++)
                //{
                //    GUILayout.BeginHorizontal();
                //    GUILayout.Label("Level of Detail Object " + j);
                //    levelsOfDetail[i] = (GameObject)EditorGUILayout.ObjectField(levelsOfDetail[i], typeof(GameObject), true, GUILayout.Width(200));
                //    GUILayout.EndHorizontal();
                //}
            }

            if (GUILayout.Button("Delete This Group", GUILayout.Width(200)))
            {
                _crowdController.RemoveGroup(group.GroupName);
            }

            

        }
    }

}


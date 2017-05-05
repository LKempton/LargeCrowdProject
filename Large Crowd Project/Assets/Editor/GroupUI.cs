using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace CrowdAI
{
    
    public class GroupUI : EditorWindow
    {
        private CrowdController _crowdController;
        private CrowdGroup[] _currentGroups;
        private CrowdGroup _unassignedGroup;

        private GUIStyle _genericTextStyle;
        private GUIStyle _headingTextStyle;
        string _newGroupName = "Group Name";

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
                    Debug.Log("Got to button activation");
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



            if (GUILayout.Button("Delete This Group", GUILayout.Width(200)))
            {
                _crowdController.RemoveGroup(group.GroupName);
            }

        }
    }

}


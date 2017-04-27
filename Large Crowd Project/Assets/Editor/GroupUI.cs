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



        void Awake()
        {
            _genericTextStyle = new GUIStyle();
            _headingTextStyle = new GUIStyle();

            _headingTextStyle.fontSize = 20;
            _headingTextStyle.padding = new RectOffset(10, 10, 10, 10);
            _headingTextStyle.fontStyle = FontStyle.Bold;

            _genericTextStyle.fontSize = 12;
            _genericTextStyle.padding = new RectOffset(10, 10, 10, 10);
            

        }


        void OnGUI()
        {
            if (_crowdController != null)
            {
                _currentGroups = _crowdController.GetGroups();
                _unassignedGroup = _crowdController.GetUnassignedGroup;

                string _newGroupName = "";

                GUILayout.Label("Add New Group",_headingTextStyle, GUILayout.Width(150));
                GUILayout.BeginHorizontal();
                GUILayout.Label("Group Name:", _genericTextStyle, GUILayout.Width(150));
                GUILayout.EndHorizontal();
                _newGroupName = GUILayout.TextField("Name");

                //start scrollable area here
                if (_unassignedGroup != null)
                {
                    ShowGroup(_unassignedGroup);
                }
                
                if (_currentGroups != null)
                {

                }
                //end scrollable area here
                
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

            
        }
    }

}


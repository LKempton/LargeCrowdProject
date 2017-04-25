using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace CrowdAI
{
    
    public class GroupUI : EditorWindow
    {
        private CrowdController _crowdController;
        private CrowdGroup[] currentGroups;
        private CrowdGroup _unassignedGroup;


        void OnGUI()
        {
            if (_crowdController != null)
            {
                currentGroups = _crowdController.GetGroups();
                _unassignedGroup = _crowdController.GetUnassignedGroup;

                ShowGroup(_unassignedGroup);


                
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


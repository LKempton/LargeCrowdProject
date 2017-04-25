using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace CrowdAI
{
    
    public class GroupUI : Editor
    {
        private CrowdController _crowdController;
        private CrowdGroup[] currentGroups;


        void OnGUI()
        {
            if (_crowdController != null)
            {
                currentGroups = _crowdController.GetGroups();
            }
        }

        [MenuItem("Window/GroupUI")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(GroupUI));
        }

        public CrowdController CrowdController
        {
            set
            {
                if (value != null)
                {
                    _crowdController = value;
                }
            }
        }
    }

}


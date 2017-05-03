using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace CrowdAI
{
    [ExecuteInEditMode]
    public class CrowdSourceCleaner : MonoBehaviour
    {
        CrowdController controller;

        void OnDestroy()
        {
            
            if (controller!= null && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                var _children = new GameObject[transform.GetChildCount()];

                for (int i = 0; i < _children.Length; i++)
                {
                    _children[i] = transform.GetChild(i).gameObject;
                }

                controller.RemoveSourceChildren(_children);
            }
            
        }


        
        public CrowdController Controller
        {
            set
            {
                if (value != null)
                {
                    controller = value;
                }
            }
        }
    }

}
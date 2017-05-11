using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdAI
{
    
    [InitializeOnLoad]
    [ExecuteInEditMode]
    public class ControllerDelegator :MonoBehaviour
    {
     static void Save()
        {
            if (Application.isPlaying & !EditorApplication.isPlayingOrWillChangePlaymode)
            {                
                return;
            }
           
                var _controller = CrowdController.GetCrowdController();

                if (_controller != null)
                {
                    _controller.SaveAll();
                }
            
            
        }

        static ControllerDelegator()
        {
            EditorApplication.playmodeStateChanged += Save;
        }

        void Awake()
        {
            if (!Application.isPlaying)
            {
                GetComponent<CrowdController>().ReadAll();
            }
        }

       
       
        
    }

}
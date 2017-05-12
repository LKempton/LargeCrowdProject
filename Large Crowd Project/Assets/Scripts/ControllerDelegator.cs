using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace CrowdAI
{
    
    [InitializeOnLoad]
    [ExecuteInEditMode]
    public class ControllerDelegator :MonoBehaviour
    {
   

       

        static ControllerDelegator()
        {
            
         
            EditorApplication.playmodeStateChanged += SaveLoad;

           
        }

        static void SaveLoad()
        {
            print("called save");
            if (Application.isPlaying & !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            var _controller = CrowdController.GetCrowdController();
            if (_controller == null)
            {
                Debug.LogError("The controller is null");
                return;
            }
           

            if (!Application.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _controller.ReadAll();
            }
            else
            {
                    _controller.SaveAll(true);
                
            }  

            
            

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
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
     static void Save()
        {
            if (Application.isPlaying & !EditorApplication.isPlayingOrWillChangePlaymode)
            {                
                return;
            }
           
                var _controller = CrowdController.GetCrowdController();

                if (_controller != null)
                {
                    _controller.SaveAll(true);
                }
            
            
        }

        static void Load(Scene scene, LoadSceneMode mode)
        {
            if (Application.isPlaying)
            {
                return;
            }

            print("I tried to load");

            var _controller = CrowdController.GetCrowdController();

            if (_controller != null)
            {
                _controller.ReadAll();
            }
        }

        static ControllerDelegator()
        {
            
            EditorSceneManager.sceneLoaded += Load;
            EditorApplication.playmodeStateChanged += SaveForPlayMode;

           // EditorSceneManager.sceneUnloaded += SaveForUnload;
        }

        static void SaveForPlayMode()
        {
            if (Application.isPlaying & !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            var _controller = CrowdController.GetCrowdController();

            if (_controller != null)
            {
                print("saved");
                _controller.SaveAll(true);
            }

        }

        static void SaveForUnload(Scene scene)
        {
            
           
            if (Application.isPlaying & !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            var _controller = CrowdController.GetCrowdController();

            if (_controller != null)
            {
                print("saved");
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdAI
{
    
    
    [ExecuteInEditMode]
    public class ControllerDelegator :MonoBehaviour
    {
      

        void Awake()
        {
           

           if (Application.isEditor & EditorApplication.isPlayingOrWillChangePlaymode)
            {
                print("Saved");
                GetComponent<CrowdController>().SaveAll();
            }
            else if (Application.isEditor)
            {
                GetComponent<CrowdController>().ReadAll();
            }



        }

        
    }

}
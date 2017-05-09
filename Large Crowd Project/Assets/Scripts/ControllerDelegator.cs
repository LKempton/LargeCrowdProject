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
            print("I am awake");

          if (Application.isEditor & !Application.isPlaying)
            {
                print("Tried to read everything");
                GetComponent<CrowdController>().ReadAll();
            }
        }

       
    }
}
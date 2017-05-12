using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdAI
{


    [ExecuteInEditMode]
    [InitializeOnLoad]
    public class PlayModeSaver : MonoBehaviour
    {


        static PlayModeSaver()
        {
            //load here

            EditorApplication.playmodeStateChanged += Save;
        }

       static void Save()
        {
            if (!Application.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                IOHandler.SaveController();
            }
        }
    }

}

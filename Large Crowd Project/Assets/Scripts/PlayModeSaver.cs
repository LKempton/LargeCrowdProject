using UnityEngine;
using UnityEditor;

namespace CrowdAI
{

    /// <summary>
    /// Class that saves when entering playmode only
    /// </summary>
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
            // This can only be true if the editor is going into playmode
            if (!Application.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                IOHandler.SaveController();
            }
        }
    }

}

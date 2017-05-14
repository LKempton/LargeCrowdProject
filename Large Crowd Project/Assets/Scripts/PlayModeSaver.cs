using UnityEngine;
using UnityEditor;

namespace CrowdAI
{

    /// <summary>
    /// Monobehaviour Class that saves when entering playmode only
    /// </summary>
    [ExecuteInEditMode]
    [InitializeOnLoad]
    public class PlayModeSaver : MonoBehaviour
    {

        /// <summary>
        /// Attaches a save void to the playmodeStaeChanged delegate when the editor launches
        /// </summary>
        static PlayModeSaver()
        {
            //load here

            EditorApplication.playmodeStateChanged += Save;
        }
        /// <summary>
        /// Attempts to save the static reference of the controller
        /// </summary>
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

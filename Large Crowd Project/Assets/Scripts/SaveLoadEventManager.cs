using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdAI
{
    /// <summary>
    /// Makes The Crowd Asset data save when the editor saves
    /// </summary>
    public class SaveLoadEventManager : UnityEditor.AssetModificationProcessor
    {
      /// <summary>
      /// Function for when the editor saves
      /// </summary>
      /// <param name="paths"> array of asset information</param>
      /// <returns> the array of paths unmodified</returns>
        static string[] OnWillSaveAssets(string[] paths)
        {
            IOHandler.SaveController();

            return paths;
        }

    }
}

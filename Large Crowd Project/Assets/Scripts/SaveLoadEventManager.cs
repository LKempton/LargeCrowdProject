using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdAI
{
    
    [InitializeOnLoad]
    public class SaveLoadEventManager : AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string[] paths)
        {
            IOHandler.SaveController();

            return paths;
        }

    }
}

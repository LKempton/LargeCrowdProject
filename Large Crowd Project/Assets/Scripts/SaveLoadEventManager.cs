using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdAI
{
    
    
    public class SaveLoadEventManager : UnityEditor.AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string[] paths)
        {
            IOHandler.SaveController();

            return paths;
        }

    }
}

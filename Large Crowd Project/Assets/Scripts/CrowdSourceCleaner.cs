using UnityEngine;
using UnityEditor;

namespace CrowdAI
{
    [ExecuteInEditMode]
    
    public class CrowdSourceCleaner : MonoBehaviour
    {
        CrowdController controller;

        
        /// <summary>
        /// When a crowd source is deleted, removes the crowd members from the crowd members list
        /// </summary>
        void OnDestroy()
        {
            
            if (controller!= null && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                var _children = new GameObject[transform.childCount];

                for (int i = 0; i < _children.Length; i++)
                {
                    _children[i] = transform.GetChild(i).gameObject;
                }

                controller.RemoveSourceChildren(_children);
            }
           

            
            
        }

      
        public CrowdController Controller
        {
            set
            {
                if (value != null)
                {
                    controller = value;
                }
            }
        }
    }

}
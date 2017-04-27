using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class CrowdSourceCleaner : MonoBehaviour
    {
        CrowdController controller;

        void OnDestroy()
        {
            controller.
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
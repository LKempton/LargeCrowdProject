﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    [ExecuteInEditMode]
    public class CrowdSourceCleaner : MonoBehaviour
    {
        CrowdController controller;

        void OnDestroy()
        {
            
            controller.CheckForNullCrowdMembers();
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
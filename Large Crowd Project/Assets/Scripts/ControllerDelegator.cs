using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    [ExecuteInEditMode]
    public class ControllerDelegator : MonoBehaviour
    {
        CrowdController _controller;

        void OnEnable()
        {
            _controller = GetComponent<CrowdController>();

            if (_controller)
            {
                //_controller.Delegate();
            }
        }

    }
}

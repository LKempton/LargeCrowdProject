using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public interface ICrowd
    {
        void SetState(string State);
        string GetState();
        void StartAnimations(float delay);
        void StartAnimation();
        void StopAnimation();
        bool LoopAnimation { get; set; }
       
        // possibly use a way of using a mix of global and local animations
    }

  
}


using UnityEngine;

namespace CrowdAI
{
    public interface ICrowd
    {
        bool SetState(string state, bool useRandDelay);
        string GetCurrentState();
      
       bool SetState(string state,float delay);
      
        void ToggleAnimation();

        GameObject Member { get; }
       

        
      
        // possibly use a way of using a mix of global and local animations
    }

  public enum CrowdFormation
    {
        CIRCLE,SQUARE, RING
    }

    public enum AnimationStatus
    {
        Playing,Stopped,Transistioning,Paused
    }

    public interface ICrowdPosition
    {
         GameObject PlaceholderObject();
         bool IsStatic();


    }
}

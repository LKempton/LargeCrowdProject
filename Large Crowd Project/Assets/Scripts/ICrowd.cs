
using UnityEngine;

/// <summary>
/// Crowd AI Asset namespace
/// </summary>
namespace CrowdAI
{
    /// <summary>
    /// Interface for crowdmembers 
    /// </summary>
    public interface ICrowd
    {
        bool SetState(string state, bool useRandDelay);
        string GetCurrentState();
      
       bool SetState(string state,float delay);
      
        void ToggleAnimation();

        GameObject Member { get; }
        // possibly use a way of using a mix of global and local animations
    }

    /// <summary>
    /// The different formations available to gerenate a crowd
    /// </summary>
  public enum CrowdFormation
    {
        CIRCLE,SQUARE, RING
    }


    /// <summary>
    /// (Not Implemented) enumerators for the current state of animation 
    /// For each crowd member
    /// </summary>
    public enum AnimationStatus
    {
        Playing,Stopped,Transistioning,Paused
    }

    public interface ICrowdPosition
    {
         GameObject PlaceholderObject();
         bool IsStatic();


    }

    /// <summary>
    /// JSON Serializeable structure to store transforms
    /// </summary>
    public struct TransFormData
    {
        public float _posX;
        public float _posY;
        public float _posZ;

        public float _rotW;
        public float _rotX;
        public float _rotY;
        public float _rotZ;

    }

    

   /// <summary>
   /// JSON serializeable structure for storing group object data
   /// </summary>
   public struct GroupData
    {
        public MemberData[] _groupMembers;
        public ModelData[] _models;
        public string _name;
    }

    /// <summary>
    /// JSON serializeable structure for storing model wrapper object data
    /// </summary>
    public struct ModelData
    {
        public string[] _modelNames;
        public int[] _sizes;
    }

    /// <summary>
    /// JSON serializeable structure for storing Crowd member data
    /// </summary>
    public struct MemberData
    {
        public TransFormData _position;
        public int source;
        
    }

    /// <summary>
    /// JSON serializeable structure for storing  Crowd Controller object data
    /// </summary>
    public struct CrowdData
    {
        public TransFormData _position;
        public string _path;
        public int _stateNameSize;
        public int _groupCount;
        public float _animationStagger;

        public string[] _stateNames;
        public GroupData[] _groups;
        public GroupData _unassignedGroup;
        public TransFormData[] _parents;
    }
}

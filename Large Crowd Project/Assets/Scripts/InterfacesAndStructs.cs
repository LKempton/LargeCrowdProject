
using UnityEngine;

/// <summary>
/// Crowd AI Asset namespace
/// </summary>
namespace CrowdAI
{
    // File for storing all interfaces and structures

    /// <summary>
    /// Interface for crowd members 
    /// </summary>
    public interface ICrowd
    {/// <summary>
    ///Sets the states of crowd members
    /// </summary>
    /// <param name="state"> the name of the state to be set to</param>
    /// <param name="useRandDelay">whether a random amount of time should be added to the delay</param>
    /// <returns> True if the crowd member is changing to the new state</returns>
        bool SetState(string state, bool useRandDelay);

        /// <summary>
        /// Gets the current state of the crowdmember
        /// </summary>
        /// <returns> The crowd members current state</returns>
        string GetCurrentState();

        /// <summary>
        ///Sets the states of crowd members
        /// </summary>
        /// <param name="state"> The name of the state to be set to</param>
        /// <param name="delay">Added delay before the crowd member switches state</param>
        ///  <returns> True if the crowd member is changing to the new state</returns>
        bool SetState(string state,float delay);

      /// <summary>
      /// Toggles the crowd member's animations
      /// </summary>
        void ToggleAnimation();

        /// <summary>
        /// Gets the gameobject associated with the crowd member
        /// </summary>
        GameObject Member { get; }
        
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

    /// <summary>
    /// Used to store Lod levels for a model
    /// </summary>
    public struct ModelWrapper
    {
        public GameObject[] _LODLevel;
    }

    /// <summary>
    /// JSON Serializeable structure to store transforms
    /// </summary>
    public struct TransformData
    {
        //Vector3 data
        public float _posX;
        public float _posY;
        public float _posZ;

        //Quaternion data
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
        //Hyperthetically load them back in using Asset class
        public string[] _modelNames;
    }

    /// <summary>
    /// JSON serializeable structure for storing Crowd member data
    /// </summary>
    public struct MemberData
    {
        public TransformData _transform;
        public int source;
        
    }

    /// <summary>
    /// JSON serializeable structure for storing  Crowd Controller object data
    /// </summary>
    public struct CrowdData
    {
        public string[] _stateNames;
        public GroupData[] _groups;
        public GroupData _unassignedGroup;
        public TransformData[] _parents;
    }
}

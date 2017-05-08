
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

    public struct SourceData
    {
        public TransFormData _position;
       
    }

   

   public struct GroupData
    {
        public TransFormData[] _groupMembers;
        public ModelData[] _models;
        public string _name;
    }

    public struct ModelData
    {
        public string[] _modelNames;
        public int[] _sizes;
    }

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
    }
}

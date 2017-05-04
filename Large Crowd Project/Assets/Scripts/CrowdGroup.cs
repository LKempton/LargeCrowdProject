using UnityEngine;
using System.Collections.Generic;


namespace CrowdAI
{
    /// <summary>
    /// Group of crowd members abritrarily seperated 
    /// </summary>
    /// 
    [System.Serializable]
    public class CrowdGroup
    {
        [SerializeField]
        private string _groupName;
        [SerializeField]
        private List<ICrowdPosition> _crowdMembers;
        private List<ModelWrapper> _models;
       

      

      

        /// <summary>
        /// Constructs a new instance of CrowdGroup
        /// </summary>
        /// <param name="groupName">The name associated with this crowd group</param>

         public CrowdGroup(string groupName)
        {
            _crowdMembers = new List<ICrowdPosition>();
            _models = new List<ModelWrapper>();
            _groupName = groupName;
        }

        /// <summary>
        /// Constructs a new instance of CrowdGroup
        /// </summary>
        /// <param name="groupName">The name associated with this crowd group</param>
        /// <param name="models"> group of Models specially for this crowdGroup</param>
   

        /// <summary>
        /// Adds a crowd member to the group
        /// </summary>
        /// <param name="crowdMember"> the crowd member game object</param>
        public void AddCrowdMember(ICrowdPosition crowdMember)
        {
            _crowdMembers.Add(crowdMember);
        }

        public void AddCrowdMember(GameObject crowdMember)
        {
            var _memberComponent = crowdMember.GetComponent<ICrowdPosition>();

            if (_memberComponent == null)
            {
                Debug.LogWarning("Prefabs don't implent ICrowdPosition interface");
                _memberComponent = crowdMember.AddComponent<CrowdMemberOptimizer>();
            }

            _crowdMembers.Add(_memberComponent);
        }
        public void AddCrowdMember(GameObject[] crowdMembers)
        {
            for (int i = 0; i < crowdMembers.Length; i++)
            {
                var _memberComponent = crowdMembers[i].GetComponent<ICrowdPosition>();

                if (_memberComponent == null)
                {
                    Debug.LogWarning("Prefabs don't implent ICrowdPosition interface");
                    _memberComponent = crowdMembers[i].AddComponent<CrowdMemberOptimizer>();
                }

                _crowdMembers.Add(_memberComponent);
            }
        }
        
      
        public void GetModels(ref string[][] modelNames, ref int[][] sizes)
        {
            if (_models.Count < 1)
            {
                return;
            }
            modelNames = new string[_models.Count][];
            sizes = new int[_models.Count][];

            for (int i = 0; i < _models.Count; i++)
            {
                var _currentModels = _models[i]._LODLevel;
                var _currentSizes = _models[i].sizes;
            }
        }

        public bool AddModelGroup(ModelWrapper model)
        {
            if (!Application.isEditor)
            {
                return false;
            }

            _models.Add(model);
            return true;

        }

        /// <summary>
        /// Sets the state of all crowd members in the gruop
        /// </summary>
        /// <param name="state">Name of the new state</param>
        /// <param name="useRandDelay"> whether there should be a delay before the animation starts </param>
        public void SetState(string state, bool useRandDelay)
        {
            
        }

        public void ToggleAnimations()
        {

        }

       /// <summary>
       /// Removes all crowd Members from the group
       /// Does not dispose of them !
       /// </summary>

        public GameObject[] ClearAllForDeletion()
        {
            var _groupMembers = _crowdMembers.ToArray();

            _crowdMembers.Clear();

            var _outGOs = new GameObject[_groupMembers.Length];
            for (int i = 0; i < _groupMembers.Length; i++)
            {
                _outGOs[i] = _groupMembers[i].PlaceholderObject();
            }

            return _outGOs;
        }
        
        public bool Remove(ICrowdPosition crowdMember)
        {
            return _crowdMembers.Remove(crowdMember);
        }

        public bool Remove(GameObject crowdMember)
        {
            if (crowdMember == null)
            {
                return false;
            }

            for (int i = 0; i < _crowdMembers.Count; i++)
            {
                if (_crowdMembers[i].PlaceholderObject() == crowdMember)
                {
                    _crowdMembers.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }


        public bool Contains(ICrowdPosition crowdMember)
        {
            return _crowdMembers.Contains(crowdMember);
        }

        public bool Contains(GameObject crowdMember)
        {
            for (int i = 0; i < _crowdMembers.Count; i++)
            {
                if(_crowdMembers[i].PlaceholderObject() == crowdMember)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The name of the group
        /// </summary>
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }

        public int Size
        {
            get
            {
                return _crowdMembers.Count;
            }
        }

      
    }

    
}

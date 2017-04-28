using UnityEngine;
using System.Collections.Generic;


namespace CrowdAI
{
    /// <summary>
    /// Group of crowd members abritrarily seperated 
    /// </summary>
    public class CrowdGroup
    {
        private string _groupName;
        private List<GameObject> _crowdMembers;

        public ModelWrapper[] _crowdModels {get; set; }


        private string[] _groupModelNames;

        /// <summary>
        /// Constructs a new instance of CrowdGroup
        /// </summary>
        /// <param name="groupName">The name associated with this crowd group</param>

         public CrowdGroup(string groupName)
        {
            _crowdMembers = new List<GameObject>();

            _groupName = groupName;
        }

        /// <summary>
        /// Constructs a new instance of CrowdGroup
        /// </summary>
        /// <param name="groupName">The name associated with this crowd group</param>
        /// <param name="models"> group of Models specially for this crowdGroup</param>
        public CrowdGroup(string groupName, string[] modelNames)
        {
            _groupName = groupName;

            _crowdMembers = new List<GameObject>();
           

        }

        /// <summary>
        /// Adds a crowd member to the group
        /// </summary>
        /// <param name="crowdMember"> the crowd member game object</param>
        public void AddCrowdMember(GameObject crowdMember)
        {
            _crowdMembers.Add(crowdMember);
        }

        public void AddCrowdMembers(GameObject[] crowdMemebers)
        {
            for (int i = 0; i < crowdMemebers.Length; i++)
            {
                _crowdMembers.Add(crowdMemebers[i]);
            }
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

            return _groupMembers;
        }
        
        public bool Remove(GameObject crowdMember)
        {
            return _crowdMembers.Remove(crowdMember);
        }
        
        public bool Contains(GameObject crowdMember)
        {
            return _crowdMembers.Contains(crowdMember);
        }

        public bool Remove(GameObject[] crowdMembers)
        {
            if (_crowdMembers.Contains(crowdMembers[0]))
            {
                for (int i = 0; i < crowdMembers.Length; i++)
                {
                    _crowdMembers.Remove(crowdMembers[i]);
                }
                return true;
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

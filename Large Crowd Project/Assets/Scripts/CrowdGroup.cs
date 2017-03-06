using UnityEngine;
using System.Collections.Generic;
using System;

namespace CrowdAI
{
    /// <summary>
    /// Group of crowd members abritrarily seperated 
    /// </summary>
    public class CrowdGroup
    {
        private string _groupName;
        private List<ICrowd> _crowdMembers;

       
        private GameObject[] _groupCrowdModels;

        /// <summary>
        /// Constructs a new instance of CrowdGroup
        /// </summary>
        /// <param name="groupName">The name associated with this crowd group</param>

         public CrowdGroup(string groupName)
        {
            _crowdMembers = new List<ICrowd>();

            _groupName = groupName;
        }

        /// <summary>
        /// Constructs a new instance of CrowdGroup
        /// </summary>
        /// <param name="groupName">The name associated with this crowd group</param>
        /// <param name="models"> group of Models specially for this crowdGroup</param>
        public CrowdGroup(string groupName, GameObject[] models)
        {
            _groupName = groupName;

            _crowdMembers = new List<ICrowd>();
            _groupCrowdModels = models;

        }

        /// <summary>
        /// Adds a crowd member to the group
        /// </summary>
        /// <param name="crowdMember"> the crowd member game object</param>
        public void AddCrowdMember(GameObject crowdMember)
        {
            _crowdMembers.Add(crowdMember.GetComponent<ICrowd>());
        }

        /// <summary>
        /// Adds a crowd member to the group
        /// </summary>
        /// <param name="crowdMeber"> The CrowdMember class associated with a game object</param>
        public void AddCrowdMember(ICrowd crowdMeber)
        {
            _crowdMembers.Add(crowdMeber);
        }

        /// <summary>
        /// Sets the state of all crowd members in the gruop
        /// </summary>
        /// <param name="state">Name of the new state</param>
        /// <param name="useRandDelay"> whether there should be a delay before the animation starts </param>
        
        public void SetState(string state, bool useRandDelay)
        {
            for (int i = 0; i < _crowdMembers.Count; i++)
            {
                _crowdMembers[i].SetState(state, useRandDelay);
            }
        }

       /// <summary>
       /// Removes all crowd Members from the group
       /// Does not dispose of them
       /// </summary>

        public void ClearAll()
        {
            _crowdMembers.Clear();
        }

        public void ToggleAnimations()
        {
            for (int i = 0; i < _crowdMembers.Count; i++)
            {
                _crowdMembers[i].ToggleAnimation();
            }
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
        }

        public GameObject[] GetCrowdModels
        {
            get
            {
                    return _groupCrowdModels;
                
                
                
            }
        }
    }

    
}

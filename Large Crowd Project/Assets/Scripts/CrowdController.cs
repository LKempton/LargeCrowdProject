using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    /// <summary>
    /// Master controlling class 
    /// </summary>
    public class CrowdController : MonoBehaviour
    {

        [SerializeField]
        private  string[] _crowdStates;
        [SerializeField]
        private string[] _groupNames;
        [SerializeField]
        private GameObject[][] _groupModels;

        private CrowdGroup[] _crowdGroups;
        [SerializeField]
        private CrowdFormation _crowdFormation;

        float animationStagger = 0.25f;

        // crowd gen parameters
        [SerializeField]
        private int _rows, _columns;
        [SerializeField]
        private float _minOffset, _maxOffset, _tiltAmount, _startHeight;
        [SerializeField]
        private GameObject[] _crowdObject;
        [SerializeField]
        bool _randomGroupDist = true;
        
      
        void Awake()
        {
            // creates a new instance of CrowdGeneration and passes in all the values (needs cleaning)
            var _generator = new CrowdGeneration(_rows,_columns,_minOffset,_maxOffset,_tiltAmount,_startHeight,_crowdObject);

       
             // If there are no groups then it will just create one
            if (_groupNames.Length<1)
            {
                _crowdGroups = new CrowdGroup[1];
                _crowdGroups[0] = new CrowdGroup("default");
            }
            else
            {// creates as many crowd groups as thier are names
                _crowdGroups = new CrowdGroup[_groupNames.Length];

                for (int i = 0; i < _groupNames.Length; i++)
                {
                   
                        _crowdGroups[i] = new CrowdGroup(_groupNames[i]);
                    
                    
                }
            }
            //All members of the crowd that are generated
            _generator.GenerateCrowd(_crowdFormation, gameObject, ref _crowdGroups, _randomGroupDist);

            

        }


       
        public string[] GetGroupNames()
        {
            var _namesCopy = new string[_groupNames.Length];

            for (int i = 0; i < _groupNames.Length; i++)
            {
                _namesCopy[i] = _groupNames[i];
            }

            return _namesCopy;
        }
      

        /// <summary>
        /// Gets all the States that any crowd member may have
        /// </summary>
        /// <returns>An array of strings that are the names of the states</returns>
        public string[] GetCrowdStates()
        {
            //copies array since they are passed by reference
            var _crowdStatesCopy = new string[_crowdStates.Length];

            for (int i = 0; i < _crowdStates.Length; i++)
            {
                _crowdStatesCopy[i] = _crowdStates[i];
            }
            return _crowdStatesCopy;
        }

        /// <summary>
        /// Searches for a state
        /// </summary>
        /// <param name="stateName">The name of the state to be searched for</param>
        /// <returns> true if the state exists</returns>
      public  bool StateExists(string stateName)
        {
            for (int i = 0; i < _crowdStates.Length; i++)
            {
                if (stateName == _crowdStates[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the animation state of all crowd members
        /// </summary>
        /// <param name="state"> The name of the animation state to be set</param>
        /// <param name="useRandDelay">whether there is a random delay between a state transistion</param>
    public void SetState(string state, bool useRandDelay)
        {
           
                for (int i = 0; i < _crowdGroups.Length; i++)
                {
                    _crowdGroups[i].SetState(state,useRandDelay);
                }
            
        }

        /// <summary>
        /// Sets the animation state of a crowd group
        /// </summary>
        /// <param name="state">Name of the animation state to be set</param>
        /// <param name="groupName">Name of the group who's animation state is changing</param>
        /// <param name="useRandDelay">whether there is a random delay between a state transistion</param>
        /// <returns> True if the state has been set sucessfully</returns>
        public bool SetState(string state, string groupName, bool useRandDelay)
        {
            for (int i = 0; i < _groupNames.Length; i++)
            {
                if (groupName == _groupNames[i])
                {
                    _crowdGroups[i].SetState(state,useRandDelay);

                    return true;
                }

            }
            return false;
        }


        public void ToggleAnimations()
        {
            for (int i = 0; i < _crowdGroups.Length; i++)
            {
                _crowdGroups[i].ToggleAnimations();
            }
        }
     



    }
}
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
        private string[] _crowdStates;
        [SerializeField]
        private int _totalCrowdMembers;
        [SerializeField]
        private string[] _groupNames;
        [SerializeField]
        private GameObject[][][] _groupModels;

       

        private CrowdGroup[] _crowdGroups;
        [SerializeField]
        private CrowdFormation _crowdFormation;

        float animationStagger = 0.25f;
       
        List<GameObject[]> _allCrowdMembers;

        // crowd gen parameters

        [SerializeField]
        private float _density, _tiltAmount, _startHeight, _innerRadius;
        
        [SerializeField]
        private GameObject _placeholderPrefab;
        [SerializeField]
        bool _randomGroupDist = true;


        private LODPoolManager _poolManager;

        [SerializeField][Range(0,1)]
        float _percentModelsPooled = 0.5f;
       

        public string[] GetGroupNames()
        {
            var _namesCopy = new string[_groupNames.Length];

            for (int i = 0; i < _groupNames.Length; i++)
            {
                _namesCopy[i] = _groupNames[i];
            }

            return _namesCopy;
        }


        void Awake()
        {
            
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
        public bool StateExists(string stateName)
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
                _crowdGroups[i].SetState(state, useRandDelay);
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
                    _crowdGroups[i].SetState(state, useRandDelay);

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


        public void GenerateCrowd()
        {
            var _parent = new GameObject();


            

            var _cleaner = _parent.AddComponent<CrowdCleaner>();
            _cleaner.Controller = this;

            var _bounds = transform.GetChild(0).transform.localPosition;

            if (_allCrowdMembers == null)
            {
                _allCrowdMembers = new List<GameObject[]>();
                _parent.name = "Crowd Source";
            }
            else
            {
                if (_allCrowdMembers.Count>0)
                {
                    _parent.name = "Crowd Source (" + _allCrowdMembers.Count + ")";
                }
                else
                {
                    _parent.name = "Crowd Source";
                }
            }


            GameObject[] _newCrowd;

            switch (_crowdFormation)
            {
                case CrowdFormation.CIRCLE:
                    _newCrowd = CrowdGen.GenCrowdCircle(_density, _parent, _bounds, _placeholderPrefab);
                    break;
                case CrowdFormation.SQUARE:
                    _newCrowd = CrowdGen.GenCrowdSquare(_density, _parent, _bounds, _placeholderPrefab);
                    break;

                 default:
                    _newCrowd = CrowdGen.GenCrowdRing(_density, _parent, _bounds,  _placeholderPrefab, _innerRadius);
                    break;
            }

            if (_newCrowd.Length > 0)
            {
                _allCrowdMembers.Add(_newCrowd);
            }

            _totalCrowdMembers = CrowdSizeTotal();

            print("Curret total = " + _totalCrowdMembers);
        }
        //All members of the crowd that are generated

        
      public int Size
        {
            get
            {
                return _totalCrowdMembers;
            }
        }

       private int CrowdSizeTotal()
        {
            int size = 0;

            for (int i = 0; i < _allCrowdMembers.Count; i++)
            {
                size += _allCrowdMembers[i].Length;
            }

            return size;
        }

        public void RemoveMembers(GameObject parent)
        {
            for (int i = 0; i < _allCrowdMembers.Count; i++)
            {
                if(_allCrowdMembers[i][0].transform.parent.gameObject == parent)
                {
                    _allCrowdMembers.RemoveAt(i);
                }
            }
        }

    }
}

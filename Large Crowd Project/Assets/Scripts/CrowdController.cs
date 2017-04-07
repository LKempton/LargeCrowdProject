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
            int _groupLength = _groupNames.Length;

            int _totalElements = 0;
            int _currentIndex = 0;

            for (int i = 0; i < _groupLength; i++)
            {
                int _modelsInGroup = _groupModels[i].Length;

                for (int j = 0; j < _modelsInGroup; j++)
                {
                    _totalElements += _groupModels[i][j].Length;
                }
            }

            var _names = new string[_totalElements];

            for (int i = 0; i < _groupLength; i++)
            {
                int _modelsInGroup = _groupModels[i].Length;

                for (int j = 0; j < _modelsInGroup; j++)
                {
                    int _LODCount = _groupModels[i][j].Length;

                    for (int k = 0; k <_LODCount ; k++)
                    {
                        _names[_currentIndex] = _groupNames[i] + "_" +j.ToString()+"_" +k.ToString();
                    }
                }
            }

            

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
            _parent.transform.position = transform.position;

            var _posModifier = Vector3.zero;

            if (_bounds.x < 0)
            {
                _posModifier.x += _bounds.x;
                _bounds.x *= -1;
            }

            if (_bounds.z < 0)
            {
                _posModifier.z += _bounds.z;
                _bounds.z *= -1;
            }

            _parent.transform.position += _posModifier;

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
                  
                    _parent.transform.position += .5f * _bounds;
                    _newCrowd = CrowdGen.GenCrowdCircle(_density, _parent, _bounds, _placeholderPrefab);
                    break;


                case CrowdFormation.SQUARE:
                    
                    _parent.transform.position += .5f * CrowdGen.GetObjectBounds(_placeholderPrefab);
                    _newCrowd = CrowdGen.GenCrowdSquare(_density, _parent, _bounds, _placeholderPrefab);
                    break;

                 default:
                    _parent.transform.position += .5f * _bounds;
                  
                    _newCrowd = CrowdGen.GenCrowdRing(_density, _parent, _bounds,  _placeholderPrefab, _innerRadius);
                    break;
            }

            if (_newCrowd.Length > 0)
            {
                _allCrowdMembers.Add(_newCrowd);
            }

         
            _totalCrowdMembers = CrowdSizeTotal();

            
        }
        //All members of the crowd that are generated
        public int GetPrediction()
        {
            int _prediction = 0;

            var _bounds = transform.GetChild(0).transform.localPosition;

            switch (_crowdFormation)
            {
                case CrowdFormation.CIRCLE:
                    _prediction = CrowdGen.EstimateCircle(_density, _bounds);
                    break;

                case CrowdFormation.RING:
                    _prediction = CrowdGen.EstimateRing(_density, _bounds, _innerRadius);

                    break;

                case CrowdFormation.SQUARE:
                    _prediction = CrowdGen.EstimateSquare(_density, _bounds);
                    break;
            }


            return _prediction;
        }
        
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
            _totalCrowdMembers = CrowdSizeTotal();
        }

    }
}

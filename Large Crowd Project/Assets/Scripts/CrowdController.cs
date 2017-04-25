using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        private GameObject[][][] _groupModels;
        [SerializeField]
        private int[][][] _pooledSizes;


        int _LODCount = 5;


        
        private List<CrowdGroup> _crowdGroups;
        private CrowdGroup _groupUnassigned;


        [SerializeField]
        private CrowdFormation _crowdFormation;

        float animationStagger = 0.25f;

        // crowd gen parameters

        [SerializeField]
        private float _density, _startHeight, _innerRadius, _rotDir = 0;

        [SerializeField]
        private GameObject _placeholderPrefab;
        [SerializeField]
        private GameObject _placeholderMesh;

        [SerializeField]
        bool _randomGroupDist = true;

        private bool placeholdersSpawned = true;


        private LODPoolManager _poolManager;

        public GameObject GetPooled(string name)
        {
            return _poolManager.GetPooledObject(name);
        }

        public string[] GetGroupNames()
        {
            if (_crowdGroups == null)
            {
                return null;
            }

            var _names = new string[_crowdGroups.Count];

            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                _names[i] = _crowdGroups[i].GroupName;
            }

            return _names;
        }

        void Awake()
        {

            int _groupLength = (_crowdGroups == null) ? 0 : _crowdGroups.Count;

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
            var _sizes = new int[_totalElements];

            var _outObjects = new GameObject[_totalElements];
           

            for (int i = 0; i < _groupLength; i++)
            {
                int _modelsInGroup = _groupModels[i].Length;

                for (int j = 0; j < _modelsInGroup; j++)
                {
                    //made LODCount global variable with set size
                    //int _LODCount = _groupModels[i][j].Length;

                    for (int k = 0; k < _LODCount; k++)
                    {
                        _names[_currentIndex] = _crowdGroups[i].GroupName + "_" + j.ToString() + "_" + k.ToString();
                        _outObjects[_currentIndex] = _groupModels[i][j][k];
                        _sizes[_currentIndex] = _pooledSizes[i][j][k];
                        _currentIndex++;
                    }
                }
            }


            _poolManager = new LODPoolManager(_sizes , _outObjects, _names);
            Debug.Log("Should have called");

        }

        public CrowdGroup[] GetGroups()
        {
            if (_crowdGroups == null)
            {
                return null;
            }

            return _crowdGroups.ToArray();
        }

        public CrowdGroup GetUnassignedGroup
        {
            get
            {
                if (_groupUnassigned == null)
                {
                    return null;
                }

                return _groupUnassigned;
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

            for (int i = 0; i < _crowdGroups.Count; i++)
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
            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                if (groupName == _crowdGroups[i].GroupName)
                {
                    _crowdGroups[i].SetState(state, useRandDelay);

                    return true;
                }

            }
            return false;
        }

        public void ToggleAnimations()
        {
            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                _crowdGroups[i].ToggleAnimations();
            }
        }

        public void AddGroup(string groupName)
        {
            var _newGroup = new CrowdGroup(groupName);

            _crowdGroups.Add(_newGroup);
        }

        public bool RemoveGroup(string groupName)
        {


            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                if (_crowdGroups[i].GroupName == groupName)
                {
                    var _group = _crowdGroups[i];

                    var _previousMembers = _group.ClearAllForDeletion();

                    _groupUnassigned.AddCrowdMembers(_previousMembers);

                    _crowdGroups.RemoveAt(i);

                    return true;
                }
            }


            return false;
        }

        public void GenerateCrowd()
        {

            var _parent = new GameObject();
            _parent.name = "Crowd Source";


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

            if (_groupUnassigned == null)
            {
                _groupUnassigned = new CrowdGroup("Unassigned");
            }

            GameObject[] _newCrowd;


            switch (_crowdFormation)
            {
                case CrowdFormation.CIRCLE:

                    _parent.transform.position += .5f * _bounds;
                    _newCrowd = CrowdGen.GenCrowdCircle(_density, _rotDir, _parent, _bounds, _placeholderPrefab);
                    break;


                case CrowdFormation.SQUARE:

                    _parent.transform.position += .5f * CrowdGen.GetObjectBounds(_placeholderPrefab);
                    _newCrowd = CrowdGen.GenCrowdSquare(_density, _rotDir, _parent, _bounds, _placeholderPrefab);
                    break;

                default:
                    _parent.transform.position += .5f * _bounds;

                    _newCrowd = CrowdGen.GenCrowdRing(_density, _rotDir, _parent, _bounds, _placeholderPrefab, _innerRadius);
                    break;
            }

            if (_newCrowd.Length > 0)
            {
                _groupUnassigned.AddCrowdMembers(_newCrowd);
            }
            else
            {
                Destroy(_parent);
            }
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
                if (_groupUnassigned == null)
                {
                    return 0;
                }

                int size = _groupUnassigned.Size;

                if (_crowdGroups != null)
                {
                    for (int i = 0; i < _crowdGroups.Count; i++)
                    {
                        size += _crowdGroups[i].Size;

                    }
                }
                return size;
            }
        }

        private void ManagePlaceholders()
        {


            placeholdersSpawned = !placeholdersSpawned;

        }

        public bool AddCrowdMembers(string groupName, GameObject[] group)
        {



            return false;
        }



    }
}

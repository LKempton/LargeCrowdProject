using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace CrowdAI
{
    /// <summary>
    /// Master controlling class 
    /// </summary>
    public class CrowdController : MonoBehaviour
    {
        private static CrowdController instance;

        private LODPoolManager _poolManager;
        
        [SerializeField]
        private string[] _crowdStates;

        private string _scene;
        private string _savePath;
        private string _fileName;

        // Whether the save/load function has been delegated to play mode event
        bool _functionDelegated = false;

        int _LODCount = 5;
        int _crowdCount = 0;

        private List<CrowdGroup> _crowdGroups;
        private List<GameObject> _crowdSources;

        private CrowdGroup _groupUnassigned;

        [SerializeField]
        bool _randomStagger;

        [SerializeField]
        private CrowdFormation _crowdFormation;

        float _animationStagger = 0.25f;

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
            ReadAll(false);
            
            if (Application.isEditor && Application.isPlaying)
            {
                EditorApplication.playmodeStateChanged -= SaveAll;
            }
            


            int _groupLength = (_crowdGroups == null) ? 0 : _crowdGroups.Count;

            int _totalElements = 0;
            int _currentIndex = 0;


            //for (int i = 0; i < _groupLength; i++)
            //{
            //    int _modelsInGroup = _groupModels[i].Length;

            //    for (int j = 0; j < _modelsInGroup; j++)
            //    {
            //        _totalElements += _groupModels[i][j].Length;
            //    }
            //}

            //var _names = new string[_totalElements];
            //var _sizes = new int[_totalElements];

            //var _outObjects = new GameObject[_totalElements];


            //for (int i = 0; i < _groupLength; i++)
            //{
            //    int _modelsInGroup = _groupModels[i].Length;

            //    for (int j = 0; j < _modelsInGroup; j++)
            //    {
            //        //made LODCount global variable with set size
            //        //int _LODCount = _groupModels[i][j].Length;

            //        for (int k = 0; k < _LODCount; k++)
            //        {
            //            _names[_currentIndex] = _crowdGroups[i].GroupName + "_" + j.ToString() + "_" + k.ToString();
            //            _outObjects[_currentIndex] = _groupModels[i][j][k];
            //            _sizes[_currentIndex] = _pooledSizes[i][j][k];
            //            _currentIndex++;
            //        }
            //    }
            //}


            //_poolManager = new LODPoolManager(_sizes, _outObjects, _names);


        }

        private CrowdData GetData()
        {
            CrowdData _outData = new CrowdData();
            _outData._path = _savePath;
            _outData._animationStagger = _animationStagger;

            _outData._position._posX = transform.position.x;
            _outData._position._posY = transform.position.y;
            _outData._position._posZ = transform.position.z;

            _outData._position._rotW = transform.rotation.w;
            _outData._position._rotX = transform.rotation.x;
            _outData._position._rotY = transform.rotation.y;
            _outData._position._rotZ = transform.rotation.z;

            if (_crowdStates != null)
            {
                _outData._stateNameSize = _crowdStates.Length;
                _outData._stateNames = _crowdStates;
            }
            if (_crowdGroups != null)
            {
                _outData._groupCount = _crowdGroups.Count;
                _outData._groups = new GroupData[_crowdGroups.Count];

                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    _outData._groups[i] = _crowdGroups[i].GetData();
                }
            }

            if (_groupUnassigned != null)
            {
                _outData._unassignedGroup = _groupUnassigned.GetData();
            }
            return _outData;
        }

        public CrowdGroup[] GetGroups()
        {
            if (_crowdGroups == null)
            {
                return null;
            }
            else
            {
                return _crowdGroups.ToArray();
            }
            
        }

        public CrowdGroup GetUnassignedGroup
        {
            get
            {
                if (_groupUnassigned == null)
                {
                    return null;
                }
                else
                {
                    return _groupUnassigned;
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
            if (_crowdGroups == null)
            {
                _crowdGroups = new List<CrowdGroup>();
            }
            _crowdGroups.Add(new CrowdGroup(groupName));

           
        }

        public void RemoveSourceChildren(GameObject[] children)
        {
            for (int i = 0; i < children.Length; i++)
            {
                var _currentChild = children[i];

                bool _childRemoved = _groupUnassigned.Remove(_currentChild);

                if (_childRemoved)
                {
                    continue;
                }
                else
                {
                    if (_crowdGroups == null)
                    {


                        for (int j = 0; j < _crowdGroups.Count; j++)
                        {
                            _childRemoved = _crowdGroups[j].Remove(_currentChild);

                            if (_childRemoved)
                            {
                                break;
                            }
                        }
                    }
                }
            }

           
        }

        public bool RemoveGroup(string groupName)
        {


            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                if (_crowdGroups[i].GroupName == groupName)
                {
                    var _group = _crowdGroups[i];

                    var _previousMembers = _group.ClearAllForDeletion();

                    if (_previousMembers != null)
                    {
                        if (_previousMembers.Length > 0)
                        {
                            _groupUnassigned.AddCrowdMember(_previousMembers);
                        }
                        
                    }

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

            var _cleaner = _parent.AddComponent<CrowdSourceCleaner>();
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

            if (_groupUnassigned == null)
            {
                SetUp();
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
                _groupUnassigned.AddCrowdMember(_newCrowd);
                _crowdCount = RecalculateCount();
                _crowdSources.Add(_parent);


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

        private int RecalculateCount()
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

        public int Size
        {
            get
            {
                return _crowdCount;
            }
        }

       void Reset()
        {
            SetUp();
        }

        public bool AddCrowdMembers(string groupName, GameObject[] group)
        {
            if (_crowdGroups == null)
            {
                return false;
            }

            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                var _currentGroup = _crowdGroups[i];

                if (_currentGroup.GroupName == groupName)
                {
                    _currentGroup.AddCrowdMember(group);
                    return true;
                }
            }

            
            return false;
        }

        public void ShowDebugInfo()
        {
            string _outInfo = "Current Crowd Count: " + _crowdCount;
            _outInfo += "\n Current Groups and the number of their members:\n";

            if (_groupUnassigned != null)
            {
                _outInfo += _groupUnassigned.GroupName + ", Size: " + _groupUnassigned.Size + "\n";
            }

            if (_crowdGroups != null)
            {
                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    _outInfo += _crowdGroups[i].GroupName + ", Size: " + _crowdGroups[i].Size + "\n";
                }
            }
            else
            {
                _outInfo += "Their are no groups \n";
            }


            _outInfo += "N. Of Crowd States:";
            if (_crowdStates != null)
            {
                _outInfo += _crowdStates.Length + "\n The States Are:";

                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    _outInfo += "\n -" + _crowdStates[i];
                }
                _outInfo += "\n";
            }
            else
            {
                _outInfo += "0 \n";
            }




            Debug.Log(_outInfo);
        }

        void SetUp()
        {
            _scene = SceneManager.GetActiveScene().name;

            if (instance != null)
            {
                if (instance != this && _scene == instance.ControllerScene)
                {
                    Debug.LogWarning("Can Only Have one Crowd Controller per scene");
                    Destroy(gameObject);
                }
            }

            instance = this;
            
            _crowdGroups = new List<CrowdGroup>();
            _groupUnassigned = new CrowdGroup("Unassigned");
            _crowdSources = new List<GameObject>();

            if (!Application.isPlaying)
            {
                EditorApplication.playmodeStateChanged += SaveAll;
                EditorApplication.playmodeStateChanged += RemovePlaceholders;
            }
        }


        public void SaveAll()
        {
            print("called save function");
            if (!Application.isEditor)
            {
                return ;
            }

            var _data = GetData();

           
                _savePath = Application.dataPath + @"/CrowdAssetData";
               _fileName = @"/CrowdData - " + SceneManager.GetActiveScene().name + ".data.json";
            

        

            if (!Directory.Exists(_savePath))
            {
                
                Directory.CreateDirectory(_savePath);
            }

            string _serializedData = JsonConvert.SerializeObject(_data);
           
            File.WriteAllText(_savePath+_fileName, _serializedData);

            
        }

        private void OverWriteData(CrowdData data, bool showPlaceholders)
        {
            if (_crowdGroups == null)
            {
                SetUp();
            }

            if (data._path != null)
            {
                _savePath = data._path;
            }
            gameObject.transform.position = new Vector3(data._position._posX, data._position._posY, data._position._posZ);
            gameObject.transform.rotation = new Quaternion(data._position._rotX, data._position._rotY, data._position._rotZ, data._position._rotW);
            _animationStagger = data._animationStagger;

            if (data._stateNameSize > 0)
            {
                _crowdStates = new string[data._stateNameSize];

                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    _crowdStates[i] = data._stateNames[i];

                }
            }
            if (showPlaceholders)
            {
               if (data._unassignedGroup._groupMembers.Length > 0)
                {
                    _groupUnassigned = GenerateGroupAndPlaceholders(data._unassignedGroup);
                }
                if (data._groupCount > 0)
                {
                    _crowdGroups.Clear();
                    int _currentGroupIndex = 0;

                    while (_currentGroupIndex < data._groupCount)
                    {

                        _crowdGroups.Add(GenerateGroupAndPlaceholders(data._groups[_currentGroupIndex]));
                        _currentGroupIndex++;
                    }
                }
            }
            else
            {
                if (data._groupCount > 0)
                {
                    for (int i = 0; i < data._groupCount; i++)
                    {
                        _crowdGroups.Add(new CrowdGroup(data._groups[i]));
                    }
                }
                if (data._unassignedGroup._groupMembers != null)
                {
                    _groupUnassigned = new CrowdGroup(data._unassignedGroup);
                }

            }

        }

        private void RemovePlaceholders()
        {
            
            if (_groupUnassigned.Size > 0)
            {
                _groupUnassigned.DestroyCrowdMembers();
                _groupUnassigned = null;
            }

            if (_crowdGroups.Count > 0)
            {
                

                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    _crowdGroups[i].DestroyCrowdMembers();
                }
                _crowdGroups.Clear();
            }
            if (_crowdSources != null)
            {
                if (_crowdSources.Count > 0)
                {
                    if (Application.isEditor)
                    {
                        for (int i = _crowdSources.Count - 1; i > -1; i--)
                        {
                            DestroyImmediate(_crowdSources[i]);

                        }
                    }
                    else
                    {
                        for (int i = _crowdSources.Count - 1; i > -1; i--)
                        {
                            Destroy(_crowdSources[i]);
                        }
                    }
                   
                    _crowdSources.Clear();
                }
            }
            
        }

        private CrowdGroup GenerateGroupAndPlaceholders(GroupData groupData)
        {
            Debug.Log("Called");
            var _newGroup = new CrowdGroup(groupData._name);
            _newGroup.OverwriteModelData(groupData._models);

            int _memberIndex = 0;

            while(_memberIndex < groupData._groupMembers.Length)
            {
                var _newMember = GameObject.Instantiate(_placeholderPrefab);
                var _transformData = groupData._groupMembers[_memberIndex];

                _newMember.transform.position = new Vector3(_transformData._posX, _transformData._posY, _transformData._posZ);
                _newMember.transform.rotation = new Quaternion(_transformData._rotX, _transformData._rotY, _transformData._rotZ, _transformData._rotW);

                _newGroup.AddCrowdMember(_newMember);
                _memberIndex++;
            }

            return _newGroup;
        }

        private void ReadAll(bool showPlaceholders)
        {
            if (_savePath == null)
            {
                _savePath = Application.dataPath + @"/CrowdAssetData/CrowdData - " + SceneManager.GetActiveScene().name + ".data.json";

            }

            if (File.Exists(_savePath))
            {
                var _reader = new StreamReader(_savePath);


                var _jsonFile = _reader.ReadToEnd();

                var data = JsonConvert.DeserializeObject<CrowdData>(_jsonFile);

                _reader.Dispose();

                OverWriteData(data,showPlaceholders);

            }
        }

        public static CrowdController GetCrowdController()
        {
            return instance;
        }

        public string ControllerScene
        {
            get
            {
                return _scene;
            }
        }
    }

}


